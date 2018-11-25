namespace NetCoreTemplate.ViewModels.Error
{
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Controllers.Admin;

    public sealed class ErrorAnonymousViewModel : BaseViewModel
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public SignInViewModel SignInViewModel { get; set; }
    }
}
