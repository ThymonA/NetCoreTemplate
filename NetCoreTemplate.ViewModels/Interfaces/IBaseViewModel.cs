namespace NetCoreTemplate.ViewModels.Interfaces
{
    using System.Collections.Generic;
    using System.Globalization;

    public interface IBaseViewModel : IBaseLabeledViewModel
    {
        int SystemUser { get; set; }

        string Language { get; set; }

        string CultureCode { get; set; }

        string Version { get; }

        CultureInfo CultureInfo { get; }

        string Path { get; set; }

        Dictionary<string, string> Config { get; set; }
    }
}
