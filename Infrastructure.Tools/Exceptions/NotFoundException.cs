using System;

namespace Infrastructure.Tools.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string objectName) : base($"Object is not found. Object name: {objectName}.")
        {
        }
    }

}
