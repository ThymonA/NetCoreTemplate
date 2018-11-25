namespace NetCoreTemplate.Authentication.Exceptions
{
    using System;

    public class DeactivatedException : Exception
    {
        public DeactivatedException()
            : base(string.Empty)
        {
        }
    }
}
