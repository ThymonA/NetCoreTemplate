namespace NetCoreTemplate.SharedKernel.Dictionary
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    public class StringValueDictionary<TValue> : Dictionary<string, TValue>
    {
        public TValue DefaultValue { get; set; }

        public StringValueDictionary()
        {
        }

        public StringValueDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public StringValueDictionary(TValue defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public new TValue this[string key]
        {
            get => TryGetValue(key, out var t) ? t : DefaultValue;
            set => base[key] = value;
        }
    }
}
