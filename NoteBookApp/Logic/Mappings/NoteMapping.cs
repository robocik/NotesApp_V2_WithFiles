using Microsoft.VisualBasic;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NoteBookApp.Logic.Domain;

namespace NoteBookApp.Logic.Mappings
{
    public class NoteMapping : ClassMapping<Note>
    {
        public NoteMapping()
        {
            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property(x => x.CreatedDateTime, map =>
            {
                map.NotNullable(true);
            });

            Property(x => x.Content, map =>
            {
                map.NotNullable(false);
                map.Length(8000);
            });

            ManyToOne(x => x.User, map =>
            {
                map.Column("UserId");
                map.NotNullable(true);
                map.Cascade(Cascade.None);
            });


            Set(x => x.Files, v =>
            {
                v.Cascade(Cascade.None);
                v.Inverse(false);
                v.Key(x =>
                {
                    x.Column("ObjectId");
                    x.ForeignKey("none");
                });
                v.Lazy(CollectionLazy.Lazy);
                v.Fetch(CollectionFetchMode.Subselect);
            }, h => h.OneToMany());
        }
    }
}