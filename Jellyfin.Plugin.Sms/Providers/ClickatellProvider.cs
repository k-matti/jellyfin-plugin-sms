using System.Net.Http;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.Sms.Providers
{
    public class ClickatellProvider : Provider, INotificationProvider
    {
        private const string Url = "https://platform.clickatell.com/v1/message"; 

        public ClickatellProvider(HttpClient httpClient,  string apiKey): base(httpClient, apiKey)
        {
        }

        public async Task<HttpResponseMessage> SendMessage(string phoneNumber, string content)
        {
            var message = $"{{\"messages\": [{{\"channel\": \"sms\",\"to\": \"{phoneNumber}\",\"content\": \"{content}\"}}]}}";

            var httpRequest = CreateHttpRequestMessage(Url, message);
            return await SendAsync(httpRequest);
        }
    }
}
