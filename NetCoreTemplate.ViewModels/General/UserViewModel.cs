namespace NetCoreTemplate.ViewModels.General
{
    using System;
    using System.Collections.Generic;

    using NetCoreTemplate.ViewModels.Base;

    public class UserViewModel : BaseAuthenticationViewModel
    {
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }

        public string ResetToken { get; set; }

        public DateTime ResetTokenDate { get; set; }

        public List<string> RoleNames { get; } = new List<string>();

        public List<RoleViewModel> Roles { get; set; }
    }
}
