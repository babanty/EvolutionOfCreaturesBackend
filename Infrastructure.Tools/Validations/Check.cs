using System;

namespace Infrastructure.Tools.Validations
{
    public static class Check
    {

        /// <param name="paramName"> nameof(param) </param>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentException" />
        public static void NotNullOrEmpty(string param, string paramName = null)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName, "String was null");
            }
            else if (param == string.Empty)
            {
                throw new ArgumentException("String was empty", paramName);
            }
        }


        /// <param name="paramName"> nameof(param) </param>
        /// <exception cref="ArgumentNullException" />
        public static void NotNull(object obj, string paramName = null)
        {
            if (obj is null)
                throw new ArgumentNullException(paramName, "Object was null");
        }


        /// <param name="paramName"> nameof(param) </param>
        /// <exception cref="ArgumentException" />
        public static void NotEmpty<T>(T structure, string paramName = null)
                            where T : struct
        {
            if (structure.Equals(default))
                throw new ArgumentException("Structure was empty", paramName);
        }
    }
}
