namespace NetCoreTemplate.SharedKernel.Expressions
{
    using System.Collections.Generic;

    public static class DictionaryExtension
    {
        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> objects)
        {
            foreach (var obj in objects)
            {
                if (dictionary.ContainsKey(obj.Key))
                {
                    dictionary[obj.Key] = obj.Value;
                }
                else
                {
                    dictionary.Add(obj.Key, obj.Value);
                }
            }
        }
    }
}
