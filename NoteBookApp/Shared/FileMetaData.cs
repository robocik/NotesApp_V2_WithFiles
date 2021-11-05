using System;
using System.ComponentModel.DataAnnotations;

namespace NoteBookApp.Shared
{
    public class FileMetaData
    {
        public FileMetaData()
        {

        }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = null!;

        [Required]
        public long FileLength { get; set; }

        public FileMetaData(string name, long fileLength, Guid objectId) 
        {
            FileName = name;
            FileLength = fileLength;
            ObjectId = objectId;
        }

        [Required]
        public Guid ObjectId { get; set; } 
    }
}