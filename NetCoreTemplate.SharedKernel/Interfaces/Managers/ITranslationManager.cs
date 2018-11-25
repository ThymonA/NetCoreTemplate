namespace NetCoreTemplate.SharedKernel.Interfaces.Managers
{
    using System;
    using System.Collections.Generic;

    using NetCoreTemplate.SharedKernel.Dictionary;

    public interface ITranslationManager
    {
        void AddOrUpdateTranslationLabel(int languageId, string module, string type, string key, string value);

        void DeleteTranslationLabel(int languageId, string module, string type, string key);

        string GetTranslationLabel(string combinedKey);

        string GetTranslationLabel(int languageId, string combinedKey);

        string GetTranslationLabel(string module, string type, string key);

        string GetTranslationLabel(int languageId, string module, string type, string key);

        StringValueDictionary<string> GetTranslationLabels(int languageId, string module, string type);

        StringValueDictionary<string> GetTranslationLabels(int languageId, string module, IEnumerable<string> types);

        StringValueDictionary<string> GetTranslationLabels(int languageId, IEnumerable<Tuple<string, string>> moduleTypes);
    }
}
