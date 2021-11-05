using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHibernate;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Interfaces;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers
{
    public abstract class HandlerBase<T> : IRequestHandler<T> where T : IRequest
    {
        private readonly ISession _session;
        protected readonly SecurityInfo _securityInfo;
        protected readonly IDateTimeProvider _dateTimeProvider;
        protected readonly IMapper _mapper;
        protected IFileSystemProvider FileSystemProvider { get; }
        private IHasFiles? DeleteFilesFor { get; set; }

        protected HandlerBase(ISession session, SecurityInfo securityInfo, IMapper mapper, IDateTimeProvider dateTimeProvider, IFileSystemProvider fileSystemProvider)
        {
            _session = session;
            _securityInfo = securityInfo;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            FileSystemProvider = fileSystemProvider;
        }
        

        protected abstract Task Execute(ISession session, T param);
        

        public async Task<Unit> Handle(T request, CancellationToken cancellationToken = default)
        {
            using (var trans = _session.BeginTransaction())
            {
                await Execute(_session, request).ConfigureAwait(false);
                await trans.CommitAsync(cancellationToken).ConfigureAwait(false);
            }

            await AfterHandle(_session).ConfigureAwait(false);
            return Unit.Value;
        }

        protected void MarkForDeleteFiles(IHasFiles obj, bool deleteFiles = false)
        {
            foreach (var file in obj.Files)
            {
                file.IsDeleted = true;
            }
            if (obj.Files.Count > 0 && deleteFiles)
            {
                DeleteFilesFor = obj;
            }

        }

        protected async Task DeleteFiles(IHasFiles param)
        {
            using (var trans = _session.BeginTransaction())
            {
                var files = await _session.QueryOver<File>().Where(x => x.IsDeleted).ListAsync().ConfigureAwait(false);
                foreach (var file in files)
                {
                    await FileSystemProvider.Delete(file.ToFileIdentifier()).ConfigureAwait(false);
                    await _session.DeleteAsync(file).ConfigureAwait(false);
                }
                await trans.CommitAsync().ConfigureAwait(false);
            }
        }

        protected virtual async Task AfterHandle(ISession session)
        {
            if (DeleteFilesFor != null)
            {
                await DeleteFiles(DeleteFilesFor).ConfigureAwait(false);
            }
        }
    }
}