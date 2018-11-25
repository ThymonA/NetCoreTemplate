namespace NetCoreTemplate.Authentication.Exceptions
{
    using System;

    public class WrongPasswordException : Exception
    {
        public WrongPasswordException()
            : base(string.Empty)
        {
        }
    }
}
