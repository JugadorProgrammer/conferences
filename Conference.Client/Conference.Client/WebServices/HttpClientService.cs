using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Conference.Client.WebServices
{
    public class HttpClientService
    {
        public static string CookieToString(Cookie cookie) 
            => $"{cookie.Name}:{cookie.Value}\n";

        public static Cookie StringToCookie(string value)
        {
            // TODO: сделать регулярное выражение для проверки value
            var splited = value.Split(':');
            if (splited.Length != 2) // TODO magic digits
            {
                throw new ArgumentException("Invalid format of `value`!");
            }

            return new Cookie(splited[0], splited[1]);
        }

        private readonly string _url;
        private readonly string _pathToConfigFile;
        private readonly CookieContainer _cookieContainer;

        public HttpClientService(IConfiguration configuration)
        {
            _cookieContainer = new CookieContainer();

            var url = configuration["Connection:Url"];
            var pathToConfigFile = configuration["CookieFileName"];

            ArgumentNullException.ThrowIfNull(url);
            ArgumentNullException.ThrowIfNull(pathToConfigFile);

            _url = url;
            _pathToConfigFile = pathToConfigFile;
            RestoreCookiesFromFile();
        }

        ~HttpClientService()
        {
            var uri = new Uri(_url);
            SaveCookieToFile(uri, _pathToConfigFile);
        }

        public async Task<HttpResponseMessage> SendRequest(string? relativeUri, HttpContent? content, HttpMethod method)
        {
            var request = new HttpRequestMessage(method, _url + relativeUri)
            {
                Content = content
            };

            using var handler = new HttpClientHandler()
            {
                CookieContainer = _cookieContainer
            };

            using var httpClient = new HttpClient(handler);
            return await httpClient.SendAsync(request);
        }

        public void SaveCookieToFile(Uri uri, string pathToFile)
        {
            var cookies = _cookieContainer.GetCookies(uri);
            using var stream = new StreamWriter(pathToFile, false);
            foreach (Cookie cookie in cookies)
            {
                var str = CookieToString(cookie);
                stream.Write(str);
            }
        }

        private void RestoreCookiesFromFile()
        {
            if (!File.Exists(_pathToConfigFile))
            {
                return;
            }

            var uri = new Uri(_url);
            using var stream = new StreamReader(_pathToConfigFile);
            while (!stream.EndOfStream)
            {
                var str = stream.ReadLine();
                if (str is null)
                {
                    break;
                }

                var cookie = StringToCookie(str);
                _cookieContainer.Add(uri, cookie);
            }
        }
    }
}
