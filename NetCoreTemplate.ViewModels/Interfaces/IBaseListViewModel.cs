namespace NetCoreTemplate.ViewModels.Interfaces
{
    using System.Collections.Generic;

    public interface IBaseListViewModel : IBaseViewModel, IBaseSearchViewModel
    {
        int PageCount { get; set; }

        int PageNumber { get; set; }

        int TotalItemCount { get; set; }

        int PageSize { get; set; }

        string DefaultOrderBy { get; set; }

        bool OrderByDesc { get; set; }
    }

    public interface IBaseListViewModel<TViewModel> : IBaseListViewModel
        where TViewModel : class, IBaseViewModel
    {
        List<TViewModel> Data { get; set; }
    }
}
