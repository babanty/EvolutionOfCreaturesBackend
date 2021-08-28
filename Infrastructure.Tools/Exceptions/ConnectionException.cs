using System;

namespace Infrastructure.Tools.Exceptions
{
    public class ConnectionException : Exception
    {
        public ConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
