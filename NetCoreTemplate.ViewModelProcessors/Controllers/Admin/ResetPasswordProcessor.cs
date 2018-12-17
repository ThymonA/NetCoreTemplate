namespace NetCoreTemplate.ViewModelProcessors.Controllers.Admin
{
    using System;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.Services.Interfaces;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.Hashing;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Controllers.Admin;

    public sealed class ResetPasswordProcessor : BaseProcessor<ResetPasswordViewModel>
    {
        private readonly IUserProvider userProvider;
        private readonly IBaseService<User> userService;

        public ResetPasswordProcessor(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.userProvider = serviceContainer.GetService<IUserProvider>();
            this.userService = serviceContainer.GetService<IBaseService<User>>();
        }

        public override void Process(ResetPasswordViewModel viewModel)
        {
            var user = userProvider.GetUserByToken(viewModel.Token);

            if (user.IsNullOrDefault() ||
                !user.ResetTokenDate.HasValue ||
                user.ResetTokenDate.Value.AddHours(12) < DateTime.Now)
            {
                return;
            }

            user.Password = PBFDF2Hash.Hash(viewModel.Password);
            user.ResetToken = string.Empty;
            user.ResetTokenDate = null;

            userService.AddOrUpdate(user);
        }
    }
}
