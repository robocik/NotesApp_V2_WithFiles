using System.Threading.Tasks;
using AutoMapper;
using NHibernate;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Interfaces;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Files
{
    public class DeleteAvatarHandler : HandlerBase<DeleteAvatarCommand>
    {
        private readonly IFileSystemProvider _fileService;


        public DeleteAvatarHandler(ISession session, SecurityInfo securityInfo, IMapper mapper, IDateTimeProvider dateTimeProvider, IFileSystemProvider fileService) : base(session, securityInfo, mapper, dateTimeProvider, fileService)
        {
            _fileService = fileService;
        }

        protected override async Task Execute(ISession session, DeleteAvatarCommand param)
        {
            var data = _securityInfo.User.ToFileIdentifier();
            await _fileService.Delete(data);

            var user = await session.GetAsync<ApplicationUser>(_securityInfo.User.Id);
            user.AvatarFile = null;
            await session.UpdateAsync(user);
        }
    }
}