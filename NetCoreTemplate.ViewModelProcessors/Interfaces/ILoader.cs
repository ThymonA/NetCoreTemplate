namespace NetCoreTemplate.ViewModelProcessors.Interfaces
{
    using NetCoreTemplate.ViewModels.Interfaces;

    public interface ILoader<out TViewModel>
        where TViewModel : class, IBaseViewModel
    {
        TViewModel Load();
    }

    public interface ILoader<out TViewModel, in TRequestModel>
        where TViewModel : class, IBaseViewModel
    {
        TViewModel Load(TRequestModel param);
    }
}
