namespace NetCoreTemplate.ViewModelProcessors.Controllers.Admin
{
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.Interfaces.Managers;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.SharedKernel.Validation;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Controllers.Admin;

    public sealed class PasswordResetValidator : BaseValidator<PasswordResetViewModel>
    {
        private readonly ITranslationManager translationManager;

        public PasswordResetValidator(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.translationManager = serviceContainer.GetService<ITranslationManager>();
        }

        public override ValidationResult Validate(PasswordResetViewModel viewModel)
        {
            var validationResult = new ValidationResult<PasswordResetViewModel>();

            if (string.IsNullOrWhiteSpace(viewModel.Email))
            {
                validationResult.AddError(
                    m => m.Email,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:Admin:EmailEmpty"));
            }

            if (!ValidEmail(viewModel.Email))
            {
                validationResult.AddError(
                    m => m.Email,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:Admin:EmailNotValid"));
            }

            return validationResult;
        }

        private static bool ValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);

                return !addr.IsNullOrDefault();
            }
            catch
            {
                return false;
            }
        }
    }
}
