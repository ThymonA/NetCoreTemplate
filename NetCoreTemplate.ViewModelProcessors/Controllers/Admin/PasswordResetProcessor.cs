namespace NetCoreTemplate.ViewModelProcessors.Controllers.Admin
{
    using System;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.Services.Interfaces;
    using NetCoreTemplate.Services.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Controllers.Admin;

    public sealed class PasswordResetProcessor : BaseProcessor<PasswordResetViewModel>
    {
        private readonly IUserProvider userProvider;
        private readonly IBaseService<User> userService;
        private readonly IMailQueueService mailQueueService;

        public PasswordResetProcessor(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.userProvider = serviceContainer.GetService<IUserProvider>();
            this.userService = serviceContainer.GetService<IBaseService<User>>();
            this.mailQueueService = serviceContainer.GetService<IMailQueueService>();
        }

        public override void Process(PasswordResetViewModel viewModel)
        {
            var user = userProvider.GetUserByEmail(viewModel.Email);

            if (user.IsNullOrDefault())
            {
                return;
            }

            user.ResetToken = RandomToken();
            user.ResetTokenDate = DateTime.Now;

            userService.AddOrUpdate(user);
        }

        private static string RandomToken() => StringExtension.GenerateRandomString(32);
    }
}
