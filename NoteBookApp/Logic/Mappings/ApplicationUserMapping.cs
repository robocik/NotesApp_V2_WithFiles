using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NoteBookApp.Logic.Domain;

namespace NoteBookApp.Logic.Mappings
{
    public class ApplicationUserMapping : JoinedSubclassMapping<ApplicationUser>
    {
        public ApplicationUserMapping()
        {
            Property(x => x.AvatarFile, map =>
            {
                map.NotNullable(false);
            });
        }
    }
}