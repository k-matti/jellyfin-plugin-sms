using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;

namespace Jellyfin.Plugin.Sms.Providers
{
    public class ClickatellProvider : INotificationProvider
    {
        private readonly string _message;
        private readonly string _apiKey;
        private const string Url = "https://platform.clickatell.com/v1/message";

        public ClickatellProvider(string phoneNumber, string content, string apiKey)
        {
            _message = $"{{\"messages\": [{{\"channel\": \"sms\",\"to\": \"{phoneNumber}\",\"content\": \"{content}\"}}]}}";
            _apiKey = apiKey;
        }

        public HttpRequestMessage CreateHttpRequestMessage()
        {
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(Url),
                Method = HttpMethod.Post,
                Content = new StringContent(
                    _message,
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            httpRequest.Headers.TryAddWithoutValidation("Authorization", _apiKey);
            return httpRequest;
        }
    }
}
