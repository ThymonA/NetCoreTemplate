namespace NetCoreTemplate.ViewModelProcessors.Controllers.User
{
    using NetCoreTemplate.DAL.Permissions;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.Interfaces.Managers;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.SharedKernel.Validation;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.General;

    public sealed class UserValidator : BaseValidator<UserViewModel>
    {
        private readonly IUserProvider userProvider;
        private readonly ITranslationManager translationManager;

        protected override bool LoadCurrentPermissions => true;

        public UserValidator(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.userProvider = serviceContainer.GetService<IUserProvider>();
            this.translationManager = serviceContainer.GetService<ITranslationManager>();
        }

        public override ValidationResult Validate(UserViewModel viewModel)
        {
            var validationResult = new ValidationResult<UserViewModel>();

            if (string.IsNullOrWhiteSpace(viewModel.Firstname))
            {
                validationResult.AddError(m => m.Firstname,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:User:FirstnameEmpty"));
            }

            if (string.IsNullOrWhiteSpace(viewModel.Lastname))
            {
                validationResult.AddError(m => m.Lastname,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:User:LastnameEmpty"));
            }

            if (viewModel.Id == default(int))
            {
                if (string.IsNullOrWhiteSpace(viewModel.Email))
                {
                    validationResult.AddError(m => m.Email,
                        translationManager.GetTranslationLabel(LanguageId, "Dashboard:User:EmailEmpty"));
                }

                var existingUser = userProvider.GetUserByEmail(viewModel.Email);

                if (!existingUser.IsNullOrDefault())
                {
                    validationResult.AddError(m => m.Email,
                        translationManager.GetTranslationLabel(LanguageId, "Dashboard:User:EmailAlreadyExists"));
                }

                return validationResult;
            }

            var editedUser = userProvider.GetUserById(viewModel.Id);

            if (editedUser.IsNullOrDefault())
            {
                validationResult.AddError(m => m.Email,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:User:UserNotFound"));

                return validationResult;
            }

            if (Actions.Contains(Permissions.GetActionKey(Module.Dashboard, Type.User, Action.Edit)))
            {
                if (string.IsNullOrWhiteSpace(viewModel.Email))
                {
                    validationResult.AddError(m => m.Email,
                        translationManager.GetTranslationLabel(LanguageId, "Dashboard:User:EmailEmpty"));
                }
                else
                {
                    var existingUser = userProvider.GetUserByEmail(viewModel.Email);

                    if (existingUser.Id != editedUser.Id)
                    {
                        validationResult.AddError(m => m.Email,
                            translationManager.GetTranslationLabel(LanguageId, "Dashboard:User:EmailAlreadyExists"));
                    }
                }
            }
            else
            {
                viewModel.Email = editedUser.Email;
            }

            return validationResult;
        }
    }
}
