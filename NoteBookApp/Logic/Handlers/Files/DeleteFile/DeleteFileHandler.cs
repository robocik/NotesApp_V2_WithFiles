using System.Threading.Tasks;
using AutoMapper;
using NHibernate;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Interfaces;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Files.DeleteFile
{
    public class DeleteFileHandler : HandlerBase<DeleteFileCommand>
    {
        public DeleteFileHandler(ISession session, SecurityInfo securityInfo, IMapper mapper, IDateTimeProvider dateTimeProvider, IFileSystemProvider fileService)
            : base(session, securityInfo, mapper, dateTimeProvider, fileService)
        {
        }

        protected override async Task Execute(ISession session, DeleteFileCommand param)
        {
            var file = await session.QueryOver<File>().Where(x=>x.Id==param.Id).SingleOrDefaultAsync().ConfigureAwait(false);
            if (file == null)
            {
                throw new NoteBookApp.Shared.Exceptions.ObjectNotFoundException("File not found");
            }


            await DeleteFile(session, file).ConfigureAwait(false);
        }

        public async Task DeleteFile(ISession session, File file)
        {
            await session.DeleteAsync(file).ConfigureAwait(false);
            var data = file.ToFileIdentifier();
            await FileSystemProvider.Delete(data).ConfigureAwait(false);
        }
    }
}