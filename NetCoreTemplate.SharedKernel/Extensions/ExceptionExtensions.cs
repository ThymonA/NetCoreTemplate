namespace NetCoreTemplate.SharedKernel.Extensions
{
    using System;

    public static class ExceptionExtensions
    {
        public static Exception GetInnermostException(this Exception e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            while (e.InnerException != null)
            {
                e = e.InnerException;
            }

            return e;
        }
    }
}
