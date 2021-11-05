using System;
using System.Threading.Tasks;
using AutoMapper;
using NHibernate;
using NHibernate.Criterion;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Interfaces;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Files
{
    public class UploadFileHandler : QueryHandlerBase<UploadFileCommand, FileAccessToken>
    {
        public UploadFileHandler(ISession session, SecurityInfo securityInfo, IMapper mapper, IDateTimeProvider dateTimeProvider, IFileSystemProvider fileService)
            : base(session, securityInfo, mapper, dateTimeProvider,fileService)
        {
        }

        protected override async Task<FileAccessToken> Execute(ISession session, UploadFileCommand param)
        {
            var file = await CreateFile(session, param);

            var fileAccessToken= await FileSystemProvider.GenerateUploadToken(_securityInfo.User.Id, file.Id.ToString());
            return _mapper.Map< FileAccessToken>(fileAccessToken);
        }

        public async Task<File> CreateFile(ISession session, UploadFileCommand param)
        {
            var hasFiles = await Validate(session, param).ConfigureAwait(false);

            var file = new File
            {
                CreatedBy = _securityInfo.User,
                FileName = param.FileName,
                CreatedDateTime = _dateTimeProvider.UtcNow,
                Length = param.FileLength,
                ObjectId = param.ObjectId,
            };

            await session.SaveAsync(file).ConfigureAwait(false);
            hasFiles.Files.Add(file);
            await session.UpdateAsync(hasFiles).ConfigureAwait(false);
            return file;
        }

        private async Task<IHasFiles> Validate(ISession session, UploadFileCommand param)
        {
            if (param.FileLength > Constants.MaxFileSize)
            {
                throw new ArgumentOutOfRangeException("File is too big");
            }

            IHasFiles? hasFiles = await session.GetAsync<Note>(param.ObjectId).ConfigureAwait(false);

            if (hasFiles == null)
            {
                throw new NoteBookApp.Shared.Exceptions.ObjectNotFoundException();
            }

            var diskSize = Constants.OneSizeMB + 100; //this can be taken from Account settings. For testing, we assume that user has 100 MB disk available
            var allFileSize = await GetAllFileSize(session, _securityInfo.User).ConfigureAwait(false);
            if (allFileSize + param.FileLength > diskSize)
            {
                throw new InvalidOperationException("No free space available");
            }

            return hasFiles;
        }

        public async Task<long> GetAllFileSize(ISession session, ApplicationUser user, Guid? parentId = null)
        {
            var query = session.QueryOver<File>().Where(x => x.CreatedBy == user && !x.IsDeleted);
            if (parentId != null)
            {
                query = query.Where(x => x.ObjectId == parentId);
            }
            var allFileSize = await query
                .Select(Projections.Sum<File>(x => x.Length)).SingleOrDefaultAsync<long>().ConfigureAwait(false);
            return allFileSize;
        }
    }
}