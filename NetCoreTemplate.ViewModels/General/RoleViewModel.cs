namespace NetCoreTemplate.ViewModels.General
{
    using System.Collections.Generic;

    using NetCoreTemplate.ViewModels.Base;

    public class RoleViewModel : BaseAuthenticationViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public List<PermissionViewModel> Permissions { get; set; }
    }
}
