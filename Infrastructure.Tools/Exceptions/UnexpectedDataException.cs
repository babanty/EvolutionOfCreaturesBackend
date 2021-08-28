using System;

namespace Infrastructure.Tools.Exceptions
{
    public class UnexpectedDataException : Exception
    {
        public UnexpectedDataException(string message) : base(message) 
        {

        }
    }
}
