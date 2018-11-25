namespace NetCoreTemplate.SharedKernel.Hashing
{
    using System;

    public class CannotPerformOperationException : Exception
    {
        public CannotPerformOperationException()
        {
        }

        public CannotPerformOperationException(string message)
            : base(message)
        {
        }

        public CannotPerformOperationException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
