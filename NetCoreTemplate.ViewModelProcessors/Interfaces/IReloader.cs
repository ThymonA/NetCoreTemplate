namespace NetCoreTemplate.ViewModelProcessors.Interfaces
{
    using NetCoreTemplate.ViewModels.Interfaces;

    public interface IReloader<TViewModel>
        where TViewModel : class, IBaseViewModel
    {
        TViewModel Reload(TViewModel viewModel);
    }
}
