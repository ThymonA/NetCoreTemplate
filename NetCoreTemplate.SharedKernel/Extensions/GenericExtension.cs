namespace NetCoreTemplate.SharedKernel.Extensions
{
    using System;

    public static class GenericExtension
    {
        public static bool IsNullOrDefault<T>(this T argument)
        {
            if (Equals(argument, default(T)))
            {
                return true;
            }

            var methodType = typeof(T);
            var underlyingType = Nullable.GetUnderlyingType(methodType);

            if (underlyingType != null && Equals(argument, Activator.CreateInstance(underlyingType)))
            {
                return true;
            }

            var argumentType = argument.GetType();

            if (argumentType.IsValueType && argumentType != methodType)
            {
                var obj = Activator.CreateInstance(argument.GetType());
                return obj.Equals(argument);
            }

            return false;
        }
    }
}
