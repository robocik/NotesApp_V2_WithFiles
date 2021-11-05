using System;
using System.Threading.Tasks;
using AutoMapper;
using NHibernate;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Interfaces;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Files
{
    public class UploadAvatarFullHandler : HandlerBase<UploadAvatarFullCommand>
    {
        private readonly IImageResizer _imageResizer;

        public UploadAvatarFullHandler(ISession session, SecurityInfo securityInfo, IMapper mapper, IDateTimeProvider dateTimeProvider, IFileSystemProvider fileService, IImageResizer imageResizer)
            : base(session, securityInfo, mapper, dateTimeProvider,fileService)
        {
            _imageResizer = imageResizer;
        }

        
        protected override async Task Execute(ISession session, UploadAvatarFullCommand param)
        {
            if (param.FileLength > Constants.AvatarSize)
            {
                throw new ArgumentOutOfRangeException("File is too big");
            }

            
            using var image =await  _imageResizer.ResizeAsync(param.Content, 300, 300).ConfigureAwait(false);

            FileIdentifier file = _securityInfo.User.ToFileIdentifier();
            await FileSystemProvider.Upload(image, file).ConfigureAwait(false);//upload to Azure

            var user = await session.GetAsync<ApplicationUser>(_securityInfo.User.Id).ConfigureAwait(false);
            user.AvatarFile = _dateTimeProvider.UtcNow.Ticks;// add current time (ticks) to have a unique number added as a parameter to avatar URL
            await session.UpdateAsync(user).ConfigureAwait(false);
        }

        
    }
}