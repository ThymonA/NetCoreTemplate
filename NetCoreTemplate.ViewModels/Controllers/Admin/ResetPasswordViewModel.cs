namespace NetCoreTemplate.ViewModels.Controllers.Admin
{
    using NetCoreTemplate.ViewModelProcessors.Base;

    public sealed class ResetPasswordViewModel : BaseViewModel
    {
        public string Email { get; set; }

        public string Token { get; set; }

        public string Password { get; set; }

        public string PasswordCheck { get; set; }
    }
}
