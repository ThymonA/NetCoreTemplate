namespace NetCoreTemplate.SharedKernel.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using MoreLinq;

    public static class EnumExtension
    {
        public static ICollection<TEnum> FillEnumCollection<TEnum>(this ICollection<TEnum> collection)
            where TEnum : struct, IConvertible
        {
            var type = typeof(TEnum);

            if (type.IsEnum)
            {
                var result = Enum.GetValues(type).Cast<TEnum>();
                result.ForEach(collection.Add);
            }

            return collection;
        }

        public static string GetDescription<TEnum>(this TEnum value)
            where TEnum : struct, IConvertible
        {
            return value
                .GetType()
                .GetMember(value.ToString(CultureInfo.InvariantCulture))
                .FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description;
        }
    }
}
