using System;
using System.Collections.Generic;
using NoteBookApp.Logic.Interfaces;

namespace NoteBookApp.Logic.Domain
{
    public class Note:NHBase,IHasFiles
    {
        public virtual string? Content { get; set; }
        public virtual DateTime CreatedDateTime { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;

        public virtual ICollection<File> Files { get; set; } = new HashSet<File>();
    }
}