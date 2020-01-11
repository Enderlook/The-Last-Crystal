using System;

namespace Additions.Exceptions
{
    public class ImpossibleStateException : InvalidOperationException
    {
        public ImpossibleStateException() { }

        public ImpossibleStateException(string message) : base(message) { }

        public ImpossibleStateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
