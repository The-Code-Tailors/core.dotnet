using System;

namespace com.fabioscagliola.Core.Presentation
{
    public class PresentationException : ApplicationException
    {
        public PresentationException() { }

        public PresentationException(string message) : base(message) { }

        public PresentationException(string message, Exception innerException) : base(message, innerException) { }

    }
}

