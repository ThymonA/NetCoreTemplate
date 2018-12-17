namespace NetCoreTemplate.ViewModelProcessors.Controllers.Admin
{
    using System;

    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Controllers.Admin;
    using NetCoreTemplate.ViewModels.Models;

    public sealed class ResetPasswordLoader : BaseLoader<ResetPasswordViewModel, string>
    {
        private readonly IUserProvider userProvider;

        public ResetPasswordLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.userProvider = serviceContainer.GetService<IUserProvider>();
        }

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Dashboard", "Admin");

        protected override ResetPasswordViewModel CreateViewModel(string token)
        {
            var user = userProvider.GetUserByToken(token);

            if (user.IsNullOrDefault() ||
                !user.ResetTokenDate.HasValue ||
                user.ResetTokenDate.Value.AddHours(12) < DateTime.Now)
            {
                return new ResetPasswordViewModel { Token = string.Empty };
            }

            return new ResetPasswordViewModel
            {
                Token = token,
                Email = user.Email
            };
        }

        protected override ResetPasswordViewModel ReloadViewModel(ResetPasswordViewModel viewModel)
        {
            viewModel.Password = string.Empty;
            viewModel.PasswordCheck = string.Empty;

            return viewModel;
        }
    }
}
