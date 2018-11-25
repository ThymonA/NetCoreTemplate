namespace NetCoreTemplate.ViewModels.Interfaces
{
    using System.Collections.Generic;

    using NetCoreTemplate.ViewModels.General;

    public interface IBaseAuthenticationListViewModel<TEntityViewModel> : IBaseListViewModel<TEntityViewModel>
        where TEntityViewModel : class, IBaseViewModel
    {
        UserViewModel User { get; set; }

        IList<string> Actions { get; set; }

        bool HasPermission(string action);
    }
}
