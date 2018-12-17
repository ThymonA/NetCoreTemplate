namespace NetCoreTemplate.ViewModels.Controllers.Admin
{
    using NetCoreTemplate.ViewModelProcessors.Base;

    public class PasswordResetViewModel : BaseViewModel
    {
        public string Email { get; set; }
    }
}
