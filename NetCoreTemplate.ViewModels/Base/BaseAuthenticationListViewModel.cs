namespace NetCoreTemplate.ViewModels.Base
{
    using System.Collections.Generic;
    using NetCoreTemplate.ViewModels.Interfaces;

    public abstract class BaseAuthenticationListViewModel<TViewModel> : BaseAuthenticationViewModel, IBaseAuthenticationListViewModel<TViewModel>
        where TViewModel : class, IBaseAuthenticationViewModel
    {
        public string SearchTerm { get; set; }

        public List<TViewModel> Data { get; set; }

        public int PageCount { get; set; }

        public int PageNumber { get; set; }

        public int TotalItemCount { get; set; }

        public int PageSize { get; set; }

        public string DefaultOrderBy { get; set; }

        public bool OrderByDesc { get; set; }
    }
}
