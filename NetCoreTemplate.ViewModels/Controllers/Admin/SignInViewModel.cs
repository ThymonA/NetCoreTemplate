namespace NetCoreTemplate.ViewModels.Controllers.Admin
{
    using NetCoreTemplate.ViewModelProcessors.Base;

    public class SignInViewModel : BaseViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
