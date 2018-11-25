namespace NetCoreTemplate.ViewModelProcessors.Base
{
    using NetCoreTemplate.SharedKernel.Validation;
    using NetCoreTemplate.ViewModelProcessors.Interfaces;
    using NetCoreTemplate.ViewModels.Interfaces;

    public abstract class BaseValidator<TViewModel> : IValidator<TViewModel>
        where TViewModel : class, IBaseViewModel
    {
        public abstract ValidationResult Validate(TViewModel viewModel);
    }
}
