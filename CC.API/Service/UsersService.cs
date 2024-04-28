using CC.API.Config;
using CC.API.Models;
using Microsoft.Extensions.Options;
using System.Net;

namespace CC.API.Service
{
    public interface IUsersService
    {
         Task<IEnumerable<User>> GetAllUsers();
    }

    public class UsersService : IUsersService
    {
        private readonly HttpClient _httpClient;
        private readonly UserApiOptions _config;

        public UsersService(HttpClient httpClient, IOptions<UserApiOptions> config)
        {
            _httpClient = httpClient;
            _config = config.Value;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var userResponse = await _httpClient.GetAsync(_config.Endpoint);

            if (userResponse.StatusCode == HttpStatusCode.NotFound) { 
                return new List<User>();
            }
            var responseContent = userResponse.Content;
            var allUsers = await responseContent.ReadFromJsonAsync<List<User>>();

            return allUsers;
        }
    }
}
