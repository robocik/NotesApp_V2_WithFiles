using System;
using System.Runtime.Serialization;

namespace NoteBookApp.Shared.Exceptions
{
    [Serializable]
    public class UniqueException : Exception
    {
        public UniqueException(string message) : base(message)
        {
        }

        public UniqueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UniqueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}