using Jellyfin.Plugin.Sms.Configuration;
using MediaBrowser.Common.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.Sms.Api
{
    [ApiController]
    [Route("Notification/Sms")]
    [Produces(MediaTypeNames.Application.Json)]
    public class SmsNotificationsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SmsNotificationsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("Test/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PostAsync([FromRoute] string userId)
        {
            var options = Plugin.Instance?.Configuration.Options.FirstOrDefault(i => string.Equals(i.UserId, userId, StringComparison.OrdinalIgnoreCase));

            if (options == null)
            {
                return BadRequest("Options are null");
            }

            var httpRequest = new HttpRequestMessage();

            if (options.Provider == Providers.Clickattel)
            {
                var message = $"{{\"messages\": [{{\"channel\": \"sms\",\"to\": \"{options.PhoneNumber}\",\"content\": \"Test notification from Jellyfin\"}}]}}";

                httpRequest = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://platform.clickatell.com/v1/message"),
                    Method = HttpMethod.Post,
                };

                httpRequest.Content = new StringContent(
                    message,
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);

                httpRequest.Headers.TryAddWithoutValidation("Authorization", options.ApiKey);
            }

            var httpClient = _httpClientFactory.CreateClient(NamedClient.Default);
            using var responseMessage = await httpClient.SendAsync(httpRequest).ConfigureAwait(false);
            return NoContent();
        }
    }
}