namespace NetCoreTemplate.Authentication.Exceptions
{
    using System;

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
            : base(string.Empty)
        {
        }

        public UnauthorizedException(string message)
            : base(message)
        {
        }

        public UnauthorizedException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public string Code { get; set; }

        public string Summary { get; set; }

        public string Link { get; set; }

        public string Id { get; set; }

        public object Causes { get; set; }
    }
}
