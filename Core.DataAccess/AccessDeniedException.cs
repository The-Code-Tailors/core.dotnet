using System;

namespace com.fabioscagliola.Core.DataAccess
{
    public class AccessDeniedException : DataAccessException
    {
        public AccessDeniedException() : base("Access denied!") { }

        public AccessDeniedException(string message) : base(message) { }

        public AccessDeniedException(string message, Exception innerException) : base(message, innerException) { }

        protected DataAccessFunction function;

        public AccessDeniedException(DataAccessFunction function)
            : base(string.Format("Access denied to the \"{0}\" function!", function))
        {
            this.function = function;
        }

        public DataAccessFunction Function { get { return function; } }

    }
}

