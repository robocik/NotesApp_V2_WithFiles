using System.Threading.Tasks;
using NoteBookApp.Shared;

namespace NoteBookApp.Client.Services
{
    public interface IAccountDataService
    {
        Task<MyProfileDto> GetMyProfile();
    }
}