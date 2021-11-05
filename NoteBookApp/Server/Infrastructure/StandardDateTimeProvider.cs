using System;
using NoteBookApp.Shared;

namespace NoteBookApp.Server.Infrastructure
{
    public class StandardDateTimeProvider:IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}