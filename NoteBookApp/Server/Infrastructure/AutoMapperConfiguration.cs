using AutoMapper;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Handlers.Files;
using NoteBookApp.Logic.Handlers.Notes;
using NoteBookApp.Shared;

namespace NoteBookApp.Server.Infrastructure
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Note, NoteDto>();
            CreateMap<GetNotesParam, GetNotesQuery>();
            CreateMap<CreateNoteParam, CreateNoteCommand>();
            CreateMap<UpdateNoteParam, UpdateNoteCommand>();
            CreateMap<GetFilesParams, GetFilesQuery>();
            CreateMap<FileMetaData, UploadFileCommand>();
            CreateMap<UploadAvatarParam, UploadAvatarFullCommand>();
            CreateMap<File, FileDto>();
            CreateMap<ApplicationUser, MyProfileDto>();
            
        }
    }
}