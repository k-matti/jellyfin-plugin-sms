using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Jellyfin.Plugin.Sms.Providers;
using MediaBrowser.Common.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Plugin.Sms.Api
{
    [ApiController]
    [Route("Notification/Sms")]
    [Produces(MediaTypeNames.Application.Json)]
    public class SmsNotificationsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public SmsNotificationsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(NamedClient.Default);
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

            INotificationProvider provider = new ProvidersFactory(options, "notification.Name").GetProvider();

            var httpRequestMessage = provider.CreateHttpRequestMessage();
            using var response = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

            return NoContent();
        }
    }
}
