using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Data.Entities;
using Jellyfin.Plugin.Sms.Configuration;
using Jellyfin.Plugin.Sms.Providers;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Notifications;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Sms
{
    public class Notifier : INotificationService
    {
        private readonly ILogger<Notifier> _logger;
        private readonly HttpClient _httpClient;

        public Notifier(ILogger<Notifier> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient(NamedClient.Default);
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

            INotificationProvider provider = new ProvidersFactory(options, notification.Name).GetProvider();

            _logger.LogDebug($"Sending sms notification using {nameof(provider)}");

            var httpRequestMessage = provider.CreateHttpRequestMessage();
            using var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug("Sms notification sent.");
            _logger.LogDebug("Sms notification service response: {0}", response.StatusCode.ToString());
        }

        private static bool IsValid(SmsOptions options)
        {
            return !string.IsNullOrEmpty(options.ApiKey);
        }
    }
}
