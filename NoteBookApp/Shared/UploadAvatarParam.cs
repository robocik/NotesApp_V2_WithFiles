using System.ComponentModel.DataAnnotations;
using System.IO;

namespace NoteBookApp.Shared
{
    public class UploadFileParam
    {
        public UploadFileParam()
        {

        }
        public UploadFileParam(string name, long fileLength)
        {
            FileName = name;
            FileLength = fileLength;
        }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = null!;

        [Required]
        public long FileLength { get; set; }

    }

    public class UploadAvatarParam : UploadFileParam
    {
        public UploadAvatarParam()
        {

        }
        public UploadAvatarParam(string name, long fileLength, Stream stream) : base(name, fileLength)
        {
            FileName = name;
            FileLength = fileLength;
            Content = stream;
        }

        [Required]
        public Stream Content { get; set; } = null!;



    }
}