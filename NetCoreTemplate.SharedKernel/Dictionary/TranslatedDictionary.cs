namespace NetCoreTemplate.SharedKernel.Dictionary
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using MoreLinq;

    [Serializable]
    public class TranslatedDictionary : Dictionary<string, string>
    {
        public TranslatedDictionary()
        {
        }

        public TranslatedDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string DefaultValue => "MISSING TRANSLATION";

        public new string this[string key]
        {
            get => TryGetValue(key, out var t) ? t : DefaultValue;
            set => base[key] = value;
        }

        public void AddRange(Dictionary<string, string> dictionary) => dictionary.ForEach(x => TryAdd(x.Key, x.Value));
    }
}
