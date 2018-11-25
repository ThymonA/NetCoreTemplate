namespace NetCoreTemplate.ViewModelProcessors.General
{
    using System;
    using System.Linq;

    using NetCoreTemplate.Authentication;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.General;
    using NetCoreTemplate.ViewModels.Models;

    public sealed class UserLoader : BaseLoader<UserViewModel, int>
    {
        private readonly IUserProvider userProvider;
        private readonly IAuthenticationClient authenticationClient;
        private readonly IRoleProvider roleProvider;

        public UserLoader(
            IServiceContainer serviceContainer,
            IUserProvider userProvider,
            IAuthenticationClient authenticationClient,
            IRoleProvider roleProvider)
            : base(serviceContainer)
        {
            this.userProvider = userProvider;
            this.authenticationClient = authenticationClient;
            this.roleProvider = roleProvider;
        }

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Dashboard", "User");

        protected override UserViewModel CreateViewModel(int param)
        {
            var user = userProvider.GetUserById(param);

            if (user.IsNullOrDefault())
            {
                return new UserViewModel
                {
                    Id = 0,
                };
            }

            var remoteInfo = authenticationClient
                .GetUserInformation(user);

            var roles = roleProvider.GetRolesForUser(user.Id);

            var vm = new UserViewModel
            {
                Id = user.Id,
                Firstname = remoteInfo.Firstname,
                Lastname = remoteInfo.Lastname,
                Username = user.Email,
                Email = remoteInfo.Email,
                ResetToken = user.ResetToken,
                ResetTokenDate = user.ResetTokenDate ?? DateTime.MinValue
            };

            vm.RoleNames.AddRange(roles.Select(x => x.Name));

            return vm;
        }

        protected override UserViewModel ReloadViewModel(UserViewModel vm)
        {
            var roles = roleProvider.GetRolesForUser(vm.Id);

            vm.RoleNames.AddRange(roles.Select(x => x.Name));

            return vm;
        }
    }
}
