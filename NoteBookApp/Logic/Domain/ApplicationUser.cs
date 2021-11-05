
using NHibernate.AspNetCore.Identity;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public virtual long? AvatarFile { get; set; }

        public virtual FileIdentifier ToFileIdentifier()
        {
            var data = new FileIdentifier(FileMetaInfo.ProfileAwatarsFolder, Id);
            return data;
        }
    }
}
