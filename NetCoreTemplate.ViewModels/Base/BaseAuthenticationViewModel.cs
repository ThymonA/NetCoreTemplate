namespace NetCoreTemplate.ViewModels.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NetCoreTemplate.DAL.Permissions;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.General;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Action = NetCoreTemplate.DAL.Permissions.Action;
    using Type = NetCoreTemplate.DAL.Permissions.Type;

    public class BaseAuthenticationViewModel : BaseViewModel, IBaseAuthenticationViewModel
    {
        public UserViewModel User { get; set; }

        public IList<string> Actions { get; set; }

        public bool HasAnyPermission(Module module, Type type, params Action[] action)
        {
            return action.Any(x => HasPermission(Permissions.GetActionKey(module, type, x)));
        }

        public bool HasPermission(Module module, Type type, Action action)
        {
            var permission = Permissions.GetActionKey(module, type, action);

            return HasPermission(permission);
        }

        public bool HasPermission(string action) => Actions.Any(x => x.Equals(action));
    }
}
