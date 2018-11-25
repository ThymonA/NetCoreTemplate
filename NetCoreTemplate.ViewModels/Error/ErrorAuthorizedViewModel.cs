namespace NetCoreTemplate.ViewModels.Error
{
    using NetCoreTemplate.ViewModels.Base;

    public class ErrorAuthorizedViewModel : BaseAuthenticationViewModel
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }
}
