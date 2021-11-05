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
    public class UpdateNoteHandler : HandlerBase<UpdateNoteCommand>
    {

        public UpdateNoteHandler(ISession session, SecurityInfo securityInfo, IMapper mapper, IDateTimeProvider dateTimeProvider, IFileSystemProvider fileSystemProvider) 
            : base(session, securityInfo, mapper, dateTimeProvider, fileSystemProvider)
        {
        }

        protected override async Task Execute(ISession session, UpdateNoteCommand param)
        {
            var note=await session.GetAsync<Note>(param.Id).ConfigureAwait(false);
            if (note == null)
            {
                throw new ObjectNotFoundException("Note not found");
            }
            
            note.Content = param.Content;
            await session.UpdateAsync(note).ConfigureAwait(false);
        }
    }
}