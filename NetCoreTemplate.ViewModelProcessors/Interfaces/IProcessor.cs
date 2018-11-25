namespace NetCoreTemplate.ViewModelProcessors.Interfaces
{
    using NetCoreTemplate.ViewModels.Interfaces;

    public interface IProcessor<in TViewModel>
        where TViewModel : class, IBaseViewModel
    {
        void Process(TViewModel viewModel);
    }
}
