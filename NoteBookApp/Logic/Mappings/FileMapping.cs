using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NoteBookApp.Logic.Domain;

namespace NoteBookApp.Logic.Mappings
{
    public class FileMapping : ClassMapping<File>
    {
        public FileMapping()
        {
            Table("Files");
            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property(x => x.FileName, map =>
            {
                map.NotNullable(true);
                map.Length(255);
            });

            Property(x => x.ObjectId, map =>
            {
                map.NotNullable(false);
                map.Update(false);
                map.Insert(false);
            });
            Property(x => x.CreatedDateTime, map =>
            {
                map.NotNullable(true);
            });
            Property(x => x.IsDeleted, map =>
            {
                map.NotNullable(true);
            });
    
            Property(x => x.Length, map =>
            {
                map.NotNullable(true);
            });
            
            ManyToOne(x => x.CreatedBy, map =>
            {
                map.Column("CreatedById");
                map.NotNullable(false);
                map.Cascade(Cascade.None);
            });
        }
    }
}