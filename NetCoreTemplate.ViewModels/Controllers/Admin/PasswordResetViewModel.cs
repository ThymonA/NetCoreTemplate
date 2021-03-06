﻿namespace NetCoreTemplate.ViewModels.Controllers.Admin
{
    using NetCoreTemplate.ViewModelProcessors.Base;

    public class PasswordResetViewModel : BaseViewModel
    {
        public string Email { get; set; }

        public string Firstname { get; set; }

        public string Token { get; set; }

        public string RootUrl { get; set; }
    }
}
