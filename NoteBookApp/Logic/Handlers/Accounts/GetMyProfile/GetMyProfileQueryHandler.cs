using System.Threading.Tasks;
using AutoMapper;
using NHibernate;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Interfaces;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Accounts
{
    public class GetMyProfileQueryHandler : QueryHandlerBase<GetMyProfileQuery, MyProfileDto>
    {

        public GetMyProfileQueryHandler(ISession session, SecurityInfo securityInfo, IMapper mapper, IDateTimeProvider dateTimeProvider,IFileSystemProvider fileSystemProvider) 
            : base(session, securityInfo, mapper, dateTimeProvider,fileSystemProvider)
        {
        }

        protected override async Task<MyProfileDto> Execute(ISession session, GetMyProfileQuery param)
        {
            var user=await session.GetAsync<ApplicationUser>(_securityInfo.User.Id);
            var profile = _mapper.Map<MyProfileDto>(user);
            if (user.AvatarFile!=null)
            {
                profile.AvatarUrl = FileSystemProvider.GetFileUrl(FileMetaInfo.ProfileAwatarsFolder, user.Id, user.AvatarFile.ToString());
            }
            
            return profile;
        }

    }
}