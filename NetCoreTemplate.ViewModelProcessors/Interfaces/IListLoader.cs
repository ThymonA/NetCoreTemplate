namespace NetCoreTemplate.ViewModelProcessors.Interfaces
{
    using NetCoreTemplate.ViewModels.Interfaces;

    public interface IListLoader<out TListViewModel>
        where TListViewModel : class, IBaseViewModel
    {
        TListViewModel Load();
    }
}
