namespace NetCoreTemplate.SharedKernel.Dictionary
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    public class KeyValueDictionary<TValue> : Dictionary<int, TValue>
    {
        public TValue DefaultValue { get; set; }

        public KeyValueDictionary()
        {
        }

        public KeyValueDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public KeyValueDictionary(TValue defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public new TValue this[int key]
        {
            get => TryGetValue(key, out var t) ? t : DefaultValue;
            set => base[key] = value;
        }
    }
}
