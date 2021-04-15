using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.Sms.Providers
{
    public class Provider
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public Provider(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public static HttpRequestMessage CreateHttpRequestMessage(string url, string message)
        {
            return  new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = new StringContent(
                    message,
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest)
        {
            httpRequest.Headers.TryAddWithoutValidation("Authorization", _apiKey);

            return await _httpClient.SendAsync(httpRequest).ConfigureAwait(false);
        }
    }
}
