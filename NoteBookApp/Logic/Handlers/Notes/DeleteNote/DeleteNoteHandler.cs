using System.Threading.Tasks;
using AutoMapper;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.Type;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Interfaces;
using NoteBookApp.Shared;
using NoteBookApp.Shared.Exceptions;
using ObjectNotFoundException = NoteBookApp.Shared.Exceptions.ObjectNotFoundException;

namespace NoteBookApp.Logic.Handlers.Notes
{
    public class DeleteNoteHandler : HandlerBase<DeleteNoteCommand>
    {

        public DeleteNoteHandler(ISession session, SecurityInfo securityInfo, IMapper mapper, IDateTimeProvider dateTimeProvider, IFileSystemProvider fileSystemProvider) : base(session, securityInfo, mapper, dateTimeProvider, fileSystemProvider)
        {
        }

        protected override async Task Execute(ISession session, DeleteNoteCommand param)
        {
            var dbNote =await  session.GetAsync<Note>(param.Id).ConfigureAwait(false);
            if (dbNote == null)
            {
                throw new ObjectNotFoundException("Note not found");
            }
            
            await session.DeleteAsync(dbNote).ConfigureAwait(false);
            MarkForDeleteFiles(dbNote);
        }
    }
}