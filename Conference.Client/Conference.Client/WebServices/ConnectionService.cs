using Conference.Core.DTOModels;
using Conference.Core.Models;
using Conference.Core.WebServices;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Conference.Client.WebServices
{
    public class ConnectionService : IConnectionService
    {
        private readonly IConfiguration _configuration;

        public ConnectionService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public async Task<bool> CreateNewUserAsync(User user)
        {
            var content = JsonContent.Create(user);
            var singInPath = _configuration["SingUp"];

            var response = await SendPostRequest(singInPath, content);
            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> ExitAsync()
        {
            var singInPath = _configuration["SignOut"];
            var response = await SendPostRequest(singInPath, default);

            return response.StatusCode == HttpStatusCode.Accepted;
        }

        public async Task<bool> IsUserVerifiedAsync(SingInModel singInModel)
        {
            var content = JsonContent.Create(singInModel);
            var singInPath = _configuration["SingIn"];

            var response = await SendPostRequest(singInPath, content);
            return response.StatusCode == HttpStatusCode.Accepted;
        }

        private async Task<HttpResponseMessage> SendPostRequest(string? requestUri, HttpContent? content)
        {
            using var client = new HttpClient();
            return await client.PostAsync(requestUri, content);
        }
    }
}
