using System;
using Jellyfin.Plugin.Sms.Configuration;
using Jellyfin.Plugin.Sms.Providers;

namespace Jellyfin.Plugin.Sms
{
    public class ProvidersFactory
    {
        private readonly SmsOptions _options;
        private readonly string _content;

        public ProvidersFactory(SmsOptions options, string content)
        {
            _options = options;
            _content = content;
        }

        public INotificationProvider GetProvider()
        {
            INotificationProvider provider = _options.Provider switch
            {
                AvailableProviders.Clickattel => new ClickatellProvider(_options.PhoneNumber, _content, _options.ApiKey),
                _ => throw new ArgumentOutOfRangeException()
            };

            return provider;
        }
    }
}
