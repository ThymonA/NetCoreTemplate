namespace NetCoreTemplate.ViewModelProcessors.Interfaces
{
    using NetCoreTemplate.SharedKernel.Validation;
    using NetCoreTemplate.ViewModels.Interfaces;

    public interface IValidator<in TViewModel>
        where TViewModel : class, IBaseViewModel
    {
        ValidationResult Validate(TViewModel viewModel);
    }
}
