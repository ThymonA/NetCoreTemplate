namespace NetCoreTemplate.DAL.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using NetCoreTemplate.DAL.Models.Translation;
    using NetCoreTemplate.DAL.PersistenceLayer;
    using NetCoreTemplate.SharedKernel.Dictionary;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.Interfaces.Managers;

    public sealed class TranslationManager : ITranslationManager
    {
        private const string DefaultLabelValue = "TRANSLATION MISSING";

        private static readonly object DictionaryLanguagesLock = new object();

        private static readonly KeyValueDictionary<StringValueDictionary<StringValueDictionary<StringValueDictionary<string>>>> DictionaryLanguages =
            new KeyValueDictionary<StringValueDictionary<StringValueDictionary<StringValueDictionary<string>>>>();

        public void AddOrUpdateTranslationLabel(int languageId, string module, string type, string key, string value)
        {
            if (IsTranslatedDictionaryLoaded(languageId, module, type))
            {
                var combinedKey = $"{module}:{type}:{key}";
                var targetDictionary = DictionaryLanguages[languageId][module][type];

                if (targetDictionary.ContainsKey(combinedKey))
                {
                    targetDictionary[combinedKey] = value;
                }
                else
                {
                    targetDictionary.Add(combinedKey, value);
                }
            }
        }

        public void DeleteTranslationLabel(int languageId, string module, string type, string key)
        {
            if (IsTranslatedDictionaryLoaded(languageId, module, type))
            {
                var combinedKey = $"{module}:{type}:{key}";

                if (DictionaryLanguages[languageId][module][type].ContainsKey(combinedKey))
                {
                    DictionaryLanguages[languageId][module][type].Remove(combinedKey);
                }
            }
        }

        public string GetTranslationLabel(string combinedKey)
        {
            return GetTranslationLabel(1, combinedKey);
        }

        public string GetTranslationLabel(int languageId, string combinedKey)
        {
            var split = combinedKey.Split(":");

            if (split.Length != 3)
            {
                return DefaultLabelValue;
            }

            var module = split[0];
            var type = split[1];
            var key = split[2];

            return GetTranslationLabel(languageId, module, type, key);
        }

        public string GetTranslationLabel(string module, string type, string key)
        {
            return GetTranslationLabel(1, module, type, key);
        }

        public string GetTranslationLabel(int languageId, string module, string type, string key)
        {
            var dictionary = GetTranslatedDictionary(languageId, module, type).Result;
            var combinedKey = $"{module}:{type}:{key}";

            return dictionary.ContainsKey(combinedKey) ? dictionary[combinedKey] : DefaultLabelValue;
        }

        public StringValueDictionary<string> GetTranslationLabels(int languageId, string module, string type)
        {
            return GetTranslatedDictionaryLabels(languageId, module, type);
        }

        public StringValueDictionary<string> GetTranslationLabels(int languageId, string module, IEnumerable<string> types)
        {
            return GetTranslationLabels(languageId, types.Select(x => new Tuple<string, string>(module, x)));
        }

        public StringValueDictionary<string> GetTranslationLabels(int languageId, IEnumerable<Tuple<string, string>> moduleTypes)
        {
            return GetTranslatedDictionaryLabels(languageId, moduleTypes);
        }

        private StringValueDictionary<string> GetTranslatedDictionaryLabels(int languageId, string module, string type)
        {
            return GetTranslatedDictionaryLabels(languageId, module, new List<string>() { type });
        }

        private StringValueDictionary<string> GetTranslatedDictionaryLabels(int languageId, string module, IEnumerable<string> types)
        {
            return GetTranslatedDictionaryLabels(languageId, types.Select(x => new Tuple<string, string>(module, x)).ToList());
        }

        private StringValueDictionary<string> GetTranslatedDictionaryLabels(int languageId, IEnumerable<Tuple<string, string>> moduleTypes)
        {
            var dictionaryListLock = new object();
            var dictionaryList = new List<StringValueDictionary<string>>();

            Parallel.ForEach(
                moduleTypes,
                x =>
                {
                    var dictionary = GetTranslatedDictionary(languageId, x.Item1, x.Item2).Result;
                    lock (dictionaryListLock)
                    {
                        dictionaryList.Add(dictionary);
                    }
                });

            return dictionaryList.CombineTranslatedDictionaries(DefaultLabelValue);
        }

        private async Task<StringValueDictionary<string>> GetTranslatedDictionary(int languageId, string module, string type)
        {
            if (!IsTranslatedDictionaryLoaded(languageId, module, type))
            {
                await LoadLabels(languageId, module, type);
            }

            return LoadTranslatedDictionary(languageId, module, type);
        }

        private bool IsTranslatedDictionaryLoaded(int languageId, string module, string type)
        {
            return DictionaryLanguages.ContainsKey(languageId) &&
                   DictionaryLanguages[languageId].ContainsKey(module) &&
                   DictionaryLanguages[languageId][module].ContainsKey(type);
        }

        private async Task LoadLabels(int languageId, string module, string type)
        {
            using (var persistence = new PersistenceLayer())
            {
                var translationLabelDefinition = await persistence
                    .Get<TranslationLabelDefinition>()
                    .Include(x => x.TranslationLabels)
                    .Where(x => x.Module == module && x.Type == type)
                    .ToListAsync();

                var definitions = translationLabelDefinition
                    .Select(x => new
                    {
                        TranslationLabelDefinition = x,
                        TranslationLabel = x.TranslationLabels
                            .FirstOrDefault(y => y.Language_Id == languageId)
                    });

                var dictionary = new StringValueDictionary<string>(DefaultLabelValue);

                foreach (var definition in definitions)
                {
                    var transDef = definition.TranslationLabelDefinition;

                    dictionary.Add($"{transDef.Module}:{transDef.Type}:{transDef.Key}", definition?.TranslationLabel?.Label ?? DefaultLabelValue);
                }

                lock (DictionaryLanguagesLock)
                {
                    if (!DictionaryLanguages.ContainsKey(languageId))
                    {
                        DictionaryLanguages.Add(languageId, new StringValueDictionary<StringValueDictionary<StringValueDictionary<string>>>());
                    }

                    if (!DictionaryLanguages[languageId].ContainsKey(module))
                    {
                        DictionaryLanguages[languageId].Add(module, new StringValueDictionary<StringValueDictionary<string>>());
                    }

                    if (!DictionaryLanguages[languageId][module].ContainsKey(type))
                    {
                        DictionaryLanguages[languageId][module].Add(type, new StringValueDictionary<string>(DefaultLabelValue));
                    }

                    DictionaryLanguages[languageId][module][type] = dictionary;
                }
            }
        }

        private StringValueDictionary<string> LoadTranslatedDictionary(int languageId, string module, string type)
        {
            if (DictionaryLanguages.ContainsKey(languageId) &&
                DictionaryLanguages[languageId].ContainsKey(module) &&
                DictionaryLanguages[languageId][module].ContainsKey(type))
            {
                return DictionaryLanguages[languageId][module][type];
            }

            return new StringValueDictionary<string>(DefaultLabelValue);
        }
    }
}
