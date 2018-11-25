namespace NetCoreTemplate.SharedKernel.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using NetCoreTemplate.SharedKernel.Dictionary;

    public static class DictionaryExtension
    {
        private static readonly object ResultLock = new object();

        public static StringValueDictionary<string> CombineTranslatedDictionaries(this List<StringValueDictionary<string>> list, string defaultValue)
        {
            var result = new StringValueDictionary<string>(defaultValue);
            foreach (var item in list)
            {
                lock (ResultLock)
                {
                    result.Merge(item);
                }
            }

            return result;
        }

        public static void Merge<TKey, TVal>(this IDictionary<TKey, TVal> result, IDictionary<TKey, TVal> value)
        {
            if (!value.IsNullOrDefault() && value.Any())
            {
                foreach (var pair in value)
                {
                    result.Add(pair.Key, pair.Value);
                }
            }
        }
    }
}
