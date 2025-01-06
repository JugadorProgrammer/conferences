using Conference.Core.DTOModels;
using Conference.Core.Models;
using Conference.Core.WebServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Conference.Client.WebServices
{
    public class ConnectionService : IConnectionService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClientService _httpClientService;

        public ConnectionService(IConfiguration configuration, HttpClientService httpClientService)
        {
            _configuration = configuration;
            _httpClientService = httpClientService;
        }

        public async Task<bool> CreateNewUserAsync(User user)
        {
            var content = JsonContent.Create(user);
            var singInPath = _configuration["Connection:SingUp"];

            var response = await _httpClientService.SendRequest(singInPath, content, HttpMethod.Post);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ExitAsync()
        {
            var singInPath = _configuration["Connection:SignOut"];
            var response = await _httpClientService.SendRequest(singInPath, default, HttpMethod.Post);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> IsUserVerifiedAsync(SingInModel singInModel)
        {
            var content = JsonContent.Create(singInModel);
            var singInPath = _configuration["Connection:SingIn"];

            var response = await _httpClientService.SendRequest(singInPath, content, HttpMethod.Post);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Group>> GetGroups(string? name)
        {
            var getGroupsUri = $"{_configuration["Connection:GetGroups"]}/{name}";

            var result = await _httpClientService.SendRequest(getGroupsUri, default, HttpMethod.Get);
            if (result.IsSuccessStatusCode)
            {
                var stringResult = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Group>>(stringResult) ?? [];
            }

            throw new InvalidOperationException("Status code is not success!");
        }

    }
}
