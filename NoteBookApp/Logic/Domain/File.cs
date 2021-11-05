using System;
using Microsoft.VisualBasic.CompilerServices;

namespace NoteBookApp.Logic.Domain
{
    public class File:NHBase
    {
        public virtual Guid? ObjectId { get; set; }
        public virtual string FileName { get; set; } = null!;
        public virtual DateTime CreatedDateTime { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; } = null!;
        
        public virtual long Length { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual FileIdentifier ToFileIdentifier()
        {
            string container = CreatedBy.Id;
            var data = new FileIdentifier(container, Id.ToString());
            return data;
        }
    }
}