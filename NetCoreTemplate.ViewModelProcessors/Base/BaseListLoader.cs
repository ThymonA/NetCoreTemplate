namespace NetCoreTemplate.ViewModelProcessors.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.SharedKernel.Dictionary;
    using NetCoreTemplate.SharedKernel.Expressions;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.Interfaces.Managers;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Interfaces;
    using NetCoreTemplate.ViewModels.Interfaces;
    using NetCoreTemplate.ViewModels.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public abstract class BaseLoaderList<TEntity, TListViewModel, TEntityViewModel>
        where TEntity : class
        where TEntityViewModel : class, IBaseViewModel
        where TListViewModel : class, IBaseListViewModel<TEntityViewModel>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;
        private readonly ITranslationManager translationManager;
        private readonly IBaseProvider<Language> languageProvider;

        protected BaseLoaderList(IServiceContainer serviceContainer)
        {
            EntityProvider = serviceContainer.GetService<IBaseProvider<TEntity>>();
            httpContextAccessor = serviceContainer.GetService<IHttpContextAccessor>();
            configuration = serviceContainer.GetService<IConfiguration>();
            translationManager = serviceContainer.GetService<ITranslationManager>();
            languageProvider = serviceContainer.GetService<IBaseProvider<Language>>();
        }

        protected readonly IBaseProvider<TEntity> EntityProvider;

        protected HttpRequest Request { get; set; }

        protected string SearchTerm { get; set; }

        protected int TotalItemCount { get; set; }

        protected int PageCount { get; set; }

        protected int PageNumber { get; set; }

        protected string QueryStringSortBy { get; set; }

        protected TranslatedDictionary Label { get; set; }

        protected virtual IQueryable<TEntity> BaseQuery => EntityProvider.GetAll();

        protected Dictionary<string, string> Config { get; set; }

        protected virtual string OrderBy => QueryStringSortBy ?? DefaultOrderBy.Body.GetStringRepresentation();

        protected virtual bool DefaultOrderByDescending => false;

        protected virtual int PageSize { get; set; }

        protected virtual bool OrderByDescending { get; set; }

        protected virtual List<Expression<Func<TEntity, object>>> IncludeList { get; } = new List<Expression<Func<TEntity, object>>>();

        protected virtual Expression<Func<TEntity, bool>> Condition { get; } = null;

        protected virtual Expression<Func<TEntity, bool>> Predicate { get; } = null;

        protected virtual Dictionary<string, List<SortExpression<TEntity>>> CustomOrderByExpressions { get; } = new Dictionary<string, List<SortExpression<TEntity>>>();

        protected abstract Expression<Func<TEntity, object>> DefaultOrderBy { get; }

        protected abstract TranslationSettings TranslationSettings { get; }

        protected int LanguageId { get; set; }

        protected string Languague { get; set; }

        protected string CultureCode { get; set; }

        protected abstract TListViewModel CreateViewModel();

        protected abstract TEntityViewModel FillViewModel(TEntity entity);

        protected virtual void LoadContextInformation()
        {
            var httpContext = httpContextAccessor.HttpContext;
            var request = httpContext.Request;

            Request = request;
            SearchTerm = Request.Query["searchTerm"];
            OrderByDescending = Request.Query["sortDescending"].ToString().ToBoolean(DefaultOrderByDescending);
            PageNumber = Request.Query["page"].ToInt();
            PageSize = Request.Query["PageSize"].ToInt(configuration["list:pagesize"].ToInt());
            QueryStringSortBy = Request.Query["sortBy"];
        }

        protected void LoadLanguage()
        {
            var languageCookie = httpContextAccessor.HttpContext.Request.Cookies
                .FirstOrDefault(x => x.Key == "language");

            var languageId = 1;

            if (!languageCookie.IsNullOrDefault())
            {
                languageId = languageCookie.Value.ToInt();
            }

            var languague = languageProvider
                .GetEntity(x => x.Id == languageId);

            if (languague.IsNullOrDefault())
            {
                LanguageId = 1;
                Languague = "NL";
                CultureCode = "nl-NL";
            }
            else
            {
                LanguageId = languague.Id;
                Languague = languague.Code;
                CultureCode = languague.CultureCode;
            }
        }

        protected TranslatedDictionary GetLabels()
        {
            var settings = TranslationSettings;
            settings.AddSettings("General", "Dashboard");
            settings.AddSettings("Dashboard", "MainMenu");

            var languageCookie = httpContextAccessor.HttpContext.Request.Cookies
                .FirstOrDefault(x => x.Key == "language");

            var languageId = 1;

            if (!languageCookie.IsNullOrDefault())
            {
                languageId = languageCookie.Value.ToInt();
            }

            var labels = translationManager.GetTranslationLabels(languageId, settings.ModuleTypes);
            var dictionary = new TranslatedDictionary();

            dictionary.AddRange(labels);

            return dictionary;
        }

        protected Dictionary<string, string> GetConfig()
        {
            var items = configuration.AsEnumerable();

            return items.ToDictionary(item => item.Key, item => item.Value);
        }

        protected virtual void ReloadContextInformation(TListViewModel listViewModel)
        {
            if (string.IsNullOrEmpty(SearchTerm))
            {
                SearchTerm = listViewModel.SearchTerm;
            }
        }

        protected virtual async Task<List<TEntity>> RetrieveData(TListViewModel listViewModel)
        {
            var list = BaseQuery;

            if (Condition != null)
            {
                list = list.Where(Condition);
            }

            SetFilterLists(list);

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                list = list.Where(Predicate);
            }

            list = ApplyFilters(list, listViewModel);

            list = ApplyOrdering(list);

            list = ApplyPaging(list);

            list = ApplyIncludes(list);

            var result = await list.ToListAsync();

            return PostProcessData(result);
        }

        protected virtual void SetFilterLists(IQueryable<TEntity> list)
        {
        }

        protected virtual IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> list, TListViewModel viewModel)
        {
            return list;
        }

        protected virtual IQueryable<TEntity> ApplyOrdering(IQueryable<TEntity> list)
        {
            if (string.IsNullOrEmpty(OrderBy))
            {
                return list;
            }

            var customExpressions = CustomOrderByExpressions;
            if (customExpressions.ContainsKey(OrderBy))
            {
                var expressionsToUse = customExpressions[OrderBy];

                return OrderByDescending
                        ? list.OrderByDescending(expressionsToUse)
                        : list.OrderBy(expressionsToUse);
            }

            return list.OrderBy(OrderBy, OrderByDescending ? "desc" : "asc");
        }

        protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> list)
        {
            var result = list;

            if (PageSize <= 0)
            {
                PageSize = 10;
            }

            if (PageNumber <= 0)
            {
                PageNumber = 1;
            }

            TotalItemCount = result.Count();
            PageCount = Math.Ceiling(TotalItemCount / (decimal)PageSize).ToInt(1);

            if (PageNumber > 1)
            {
                result = result.Skip((PageNumber - 1) * PageSize);
            }

            if (PageSize * PageNumber <= TotalItemCount)
            {
                result = result.Take(PageSize);
            }

            return result;
        }

        protected IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> list)
        {
            var includeList = IncludeList;

            includeList?.ForEach(x => list = list.Include(x));

            return list;
        }

        protected virtual List<TEntity> PostProcessData(List<TEntity> list)
        {
            return list;
        }

        protected TListViewModel FinalizeViewModel(TListViewModel viewModel)
        {
            AddLanguague(viewModel);
            AddLabels(viewModel);
            AddListInfo(viewModel);
            AddConfig(viewModel);
            AddPath(viewModel);
            AddPageSize(viewModel);
            AddBaseOrderBy(viewModel);

            return viewModel;
        }

        protected void AddBaseOrderBy(TListViewModel viewModel)
        {
            viewModel.DefaultOrderBy = DefaultOrderBy.Body.GetStringRepresentation();
            viewModel.OrderByDesc = OrderByDescending;
        }

        protected void AddLabels(TListViewModel viewModel)
        {
            viewModel.Label = Label;
        }

        protected void AddListInfo(TListViewModel viewModel)
        {
            viewModel.PageCount = PageCount;
            viewModel.PageNumber = PageNumber;
            viewModel.TotalItemCount = TotalItemCount;
            viewModel.SearchTerm = SearchTerm;
        }

        protected void AddConfig<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IBaseViewModel
        {
            viewModel.Config = Config;
        }

        protected void AddPath<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IBaseViewModel
        {
            viewModel.Path = httpContextAccessor.HttpContext.Request.Path.Value;
        }

        protected void AddLanguague<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IBaseViewModel
        {
            viewModel.Language = Languague;
            viewModel.CultureCode = CultureCode;
        }

        protected void AddPageSize(TListViewModel viewModel)
        {
            viewModel.PageSize = PageSize;
        }
    }

    public abstract class BaseListLoader<TEntity, TListViewModel, TEntityViewModel> : 
        BaseLoaderList<TEntity, TListViewModel, TEntityViewModel>,
        IListLoader<TListViewModel>,
        IReloader<TListViewModel>
        where TEntity : class
        where TEntityViewModel : class, IBaseViewModel
        where TListViewModel : class, IBaseListViewModel<TEntityViewModel>
    {
        protected BaseListLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        public TListViewModel Load()
        {
            LoadLanguage();
            LoadContextInformation();

            Label = GetLabels();
            Config = GetConfig();

            var viewModel = CreateViewModel();

            ReloadContextInformation(viewModel);

            var data = RetrieveData(viewModel).Result;
            var viewModelData = data.Select(FillViewModel).ToList();

            viewModel.Data = viewModelData;

            return FinalizeViewModel(viewModel);
        }

        public TListViewModel Reload(TListViewModel listViewModel) => Load();
    }
}
