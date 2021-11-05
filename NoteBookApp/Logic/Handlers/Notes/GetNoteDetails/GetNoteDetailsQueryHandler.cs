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
    public class GetNoteDetailsQueryHandler : QueryHandlerBase<GetNoteDetailsQuery,NoteDto>
    {

        protected override async Task<NoteDto> Execute(ISession session, GetNoteDetailsQuery param)
        {
            var note = await session.GetAsync<Note>(param.Id).ConfigureAwait(false);

            if (note == null)
            {
                throw new ObjectNotFoundException("Note not found");
            }

            return _mapper.Map<NoteDto>(note);
        }
        

        public GetNoteDetailsQueryHandler(ISession session, SecurityInfo securityInfo, IMapper mapper, IDateTimeProvider dateTimeProvider,IFileSystemProvider fileService) : base(session, securityInfo, mapper, dateTimeProvider, fileService)
        {
        }

    }
}