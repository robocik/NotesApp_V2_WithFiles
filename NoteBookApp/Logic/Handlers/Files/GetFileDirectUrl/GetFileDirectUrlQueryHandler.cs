using System.Threading.Tasks;
using AutoMapper;
using NHibernate;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Interfaces;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Files
{
    public class GetFileDirectUrlQueryHandler : QueryHandlerBase<GetFileDirectUrlQuery, string>
    {

        public GetFileDirectUrlQueryHandler(ISession session, SecurityInfo securityInfo, IMapper mapper,
            IDateTimeProvider dateTimeProvider, IFileSystemProvider fileService) : base(session, securityInfo, mapper, dateTimeProvider, fileService)
        {
        }

        protected override async Task<string> Execute(ISession session, GetFileDirectUrlQuery param)
        {
            var query = session.QueryOver<File>()
                .Where(x => x.Id==param.Id && !x.IsDeleted);
            var file = await query.SingleOrDefaultAsync().ConfigureAwait(false);
            if (file==null)
            {
                throw new Shared.Exceptions.ObjectNotFoundException("File not found");
            }

            var url= FileSystemProvider.GenerateReadToken(file.CreatedBy.Id,file.Id.ToString(),file.FileName);
            return url;
        }

        
    }
}