namespace NetCoreTemplate.ViewModelProcessors.Base
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using NetCoreTemplate.SharedKernel.Dictionary;
    using NetCoreTemplate.ViewModels.Interfaces;

    public abstract class BaseViewModel : IBaseViewModel
    {
        public int SystemUser { get; set; }

        public string Language { get; set; }

        public string CultureCode { get; set; }

        public CultureInfo CultureInfo =>
            string.IsNullOrEmpty(CultureCode)
                ? new CultureInfo("nl-NL")
                : new CultureInfo(CultureCode);

        public string Path { get; set; }

        public string Version => Config.FirstOrDefault(x => x.Key.Equals(nameof(Version))).Value ?? string.Empty;

        public TranslatedDictionary Label { get; set; } = new TranslatedDictionary();

        public Dictionary<string, string> Config { get; set; } = new Dictionary<string, string>();
    }
}
