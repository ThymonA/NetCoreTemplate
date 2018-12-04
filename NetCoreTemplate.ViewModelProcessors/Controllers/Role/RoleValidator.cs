namespace NetCoreTemplate.ViewModelProcessors.Controllers.Role
{
    using System.Collections.Generic;
    using System.Linq;

    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.Interfaces.Managers;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.SharedKernel.Validation;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.General;

    public sealed class RoleValidator : BaseValidator<RoleViewModel>
    {
        private readonly IPermissionProvider permissionProvider;
        private readonly ITranslationManager translationManager;

        public RoleValidator(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.permissionProvider = serviceContainer.GetService<IPermissionProvider>();
            this.translationManager = serviceContainer.GetService<ITranslationManager>();
        }

        public override ValidationResult Validate(RoleViewModel viewModel)
        {
            var validationResult = new ValidationResult<RoleViewModel>();
            var permissions = this.permissionProvider.GetAll().ToList();

            if (viewModel.Permissions.IsNullOrDefault() || viewModel.Permissions.Count == default(int))
            {
                viewModel.Permissions = new List<PermissionViewModel>();
            }

            if (string.IsNullOrWhiteSpace(viewModel.Name))
            {
                validationResult.AddError(role => role.Name,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:Role:NameEmpty"));
            }

            for (var i = 0; i < viewModel.Permissions.Count; i++)
            {
                if (!permissions.Any(y => y.Id == viewModel.Permissions[i].Id))
                {
                    validationResult.AddError($"Permissions[{i}].Action",
                        translationManager.GetTranslationLabel(LanguageId, "Dashboard:Role:PermissionNotExists"));
                }
            }

            return validationResult;
        }
    }
}
