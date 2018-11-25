namespace NetCoreTemplate.ViewModels.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class TranslationSettings
    {
        public List<Tuple<string, string>> ModuleTypes { get; } = new List<Tuple<string, string>>();

        public TranslationSettings(string module, params string[] types)
        {
            AddSettings(module, types);
        }

        public TranslationSettings(IEnumerable<Tuple<string, string>> moduleTypes)
        {
            AddSettings(moduleTypes);
        }

        public void AddSettings(string module, params string[] types)
        {
            AddSettings(types.Select(x => new Tuple<string, string>(module, x)));
        }

        public void AddSettings(IEnumerable<Tuple<string, string>> moduleTypes)
        {
            ModuleTypes.AddRange(moduleTypes);
        }
    }
}
