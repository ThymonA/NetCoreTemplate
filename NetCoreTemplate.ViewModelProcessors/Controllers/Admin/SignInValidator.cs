namespace NetCoreTemplate.ViewModelProcessors.Controllers.Admin.SignIn
{
    using NetCoreTemplate.SharedKernel.Interfaces.Managers;
    using NetCoreTemplate.SharedKernel.Validation;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Controllers.Admin;

    public sealed class SignInValidator : BaseValidator<SignInViewModel>
    {
        private readonly ITranslationManager translationManager;

        public SignInValidator(ITranslationManager translationManager)
        {
            this.translationManager = translationManager;
        }

        public override ValidationResult Validate(SignInViewModel viewModel)
        {
            var validationResult = new ValidationResult<SignInViewModel>();

            if (string.IsNullOrWhiteSpace(viewModel.Username))
            {
                validationResult.AddError(
                    m => m.Username,
                    translationManager.GetTranslationLabel("Admin", "SignIn", "EmailEmpty"));
            }

            if (string.IsNullOrWhiteSpace(viewModel.Password))
            {
                validationResult.AddError(
                    m => m.Password,
                    translationManager.GetTranslationLabel("Admin", "SignIn", "PasswordEmpty"));
            }

            return validationResult;
        }
    }
}
