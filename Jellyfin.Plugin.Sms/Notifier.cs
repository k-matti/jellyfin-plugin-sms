using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Data.Entities;
using Jellyfin.Plugin.Sms.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Notifications;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Sms
{
    public class Notifier : INotificationService
    {
        private readonly ILogger<Notifier> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public Notifier(ILogger<Notifier> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public bool IsEnabledForUser(User user)
        {
            var options = GetOptions(user);

            return options != null && IsValid(options) && options.Enabled;
        }

        private static SmsOptions GetOptions(User user)
        {
            return Plugin.Instance?.Configuration.Options.FirstOrDefault(i => string.Equals(i.UserId, user.Id.ToString(), StringComparison.OrdinalIgnoreCase));
        }

        public string Name => Plugin.Instance.Name;

        public async Task SendNotification(UserNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Sending sms notification...");
            var options = GetOptions(notification.User);
            var httpRequest = new HttpRequestMessage();

            if (options.Provider == Providers.Clickattel)
            {
                _logger.LogDebug("Sending sms notification using Clickattel provider");

                var message = $"{{\"messages\": [{{\"channel\": \"sms\",\"to\": \"{options.PhoneNumber}\",\"content\": \"{notification.Name}\"}}]}}";

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

            using (var response = await httpClient.SendAsync(httpRequest).ConfigureAwait(false))
            {
                _logger.LogDebug("Sms notification sent.");
                _logger.LogDebug("Sms notification service response: {0}", response.StatusCode.ToString());
            }
        }

        private static bool IsValid(SmsOptions options)
        {
            return !string.IsNullOrEmpty(options.ApiKey);
        }
    }
}