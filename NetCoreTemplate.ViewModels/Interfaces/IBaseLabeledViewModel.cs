namespace NetCoreTemplate.ViewModels.Interfaces
{
    using System.Collections.Generic;

    using NetCoreTemplate.SharedKernel.Dictionary;

    public interface IBaseLabeledViewModel
    {
        TranslatedDictionary Label { get; set; }
    }
}
