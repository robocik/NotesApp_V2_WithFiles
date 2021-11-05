using System;

namespace NoteBookApp.Logic.Domain
{
    public class FileMetaInfo
    {
        public const string ProfileAwatarsFolder = "profile";

        public FileMetaInfo(string file, long length)
        {
            File = file;
            Length = length;
        }

        public string File { get; }

        public long Length { get; }


    }
}