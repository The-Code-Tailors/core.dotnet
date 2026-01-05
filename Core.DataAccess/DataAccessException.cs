using System;

namespace com.fabioscagliola.Core.DataAccess
{
    public class DataAccessException : ApplicationException
    {
        public DataAccessException() { }

        public DataAccessException(string message) : base(message) { }

        public DataAccessException(string message, Exception innerException) : base(message, innerException) { }

    }
}

