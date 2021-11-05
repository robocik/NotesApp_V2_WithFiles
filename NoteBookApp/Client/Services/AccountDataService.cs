using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NoteBookApp.Shared;

namespace NoteBookApp.Client.Services
{
    public class AccountDataService: DataServiceBase,IAccountDataService
    {
        public AccountDataService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<MyProfileDto> GetMyProfile()
        {
            return await Execute(async httpClient =>
            {
                var res = await httpClient.GetFromJsonAsync<MyProfileDto>("api/account/profile", CreateOptions());
                return res!;
            });
        }
    }
}