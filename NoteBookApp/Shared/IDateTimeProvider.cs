using System;

namespace NoteBookApp.Shared
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}