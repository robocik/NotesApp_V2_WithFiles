using System;
using System.Collections.Generic;
using NoteBookApp.Logic.Domain;

namespace NoteBookApp.Logic.Interfaces
{
    public interface IIdObject
    {
        Guid Id { get; }
    }
    public interface IHasFiles : IIdObject
    {
        ICollection<File> Files { get; }
    }
}