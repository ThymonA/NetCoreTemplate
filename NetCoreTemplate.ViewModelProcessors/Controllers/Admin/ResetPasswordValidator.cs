namespace NetCoreTemplate.ViewModelProcessors.Controllers.Admin
{
    using NetCoreTemplate.SharedKernel.Enums;
    using NetCoreTemplate.SharedKernel.Hashing;
    using NetCoreTemplate.SharedKernel.Interfaces.Managers;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.SharedKernel.Validation;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Controllers.Admin;

    public sealed class ResetPasswordValidator : BaseValidator<ResetPasswordViewModel>
    {
        private readonly ITranslationManager translationManager;

        public ResetPasswordValidator(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.translationManager = serviceContainer.GetService<ITranslationManager>();
        }

        public override ValidationResult Validate(ResetPasswordViewModel viewModel)
        {
            var validationResult = new ValidationResult<ResetPasswordViewModel>();

            if (string.IsNullOrWhiteSpace(viewModel.Token))
            {
                validationResult.AddError(
                    x => x.Token,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:Admin:EmptyToken"));
            }

            if (!string.Equals(viewModel.Password, viewModel.PasswordCheck))
            {
                validationResult.AddError(
                    x => x.Password,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:Admin:PasswordMismatch"));
                validationResult.AddError(
                    x => x.PasswordCheck,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:Admin:PasswordMismatch"));
            }

            if (string.IsNullOrWhiteSpace(viewModel.Password))
            {
                validationResult.AddError(
                    x => x.Password,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:Admin:EmptyPassword"));
            }

            if (string.IsNullOrWhiteSpace(viewModel.PasswordCheck))
            {
                validationResult.AddError(
                    x => x.PasswordCheck,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:Admin:EmptyPassword"));
            }

            if (PasswordStrengthCheck.GetPasswordStrength(viewModel.Password) != PasswordStrength.VeryStrong)
            {
                validationResult.AddError(
                    m => m.Password,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:Admin:WeakPassword"));
            }

            return validationResult;
        }
    }
}
