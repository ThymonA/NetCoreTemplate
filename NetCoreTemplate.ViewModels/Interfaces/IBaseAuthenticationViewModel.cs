namespace NetCoreTemplate.ViewModels.Interfaces
{
    using System.Collections.Generic;

    using NetCoreTemplate.DAL.Permissions;
    using NetCoreTemplate.ViewModels.General;

    public interface IBaseAuthenticationViewModel : IBaseViewModel
    {
        UserViewModel User { get; set; }

        IList<string> Actions { get; set; }

        bool HasPermission(Module module, Type type, Action action);

        bool HasPermission(string action);
    }
}
