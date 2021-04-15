using System;
using System.Net.Http;
using Jellyfin.Plugin.Sms.Providers;

namespace Jellyfin.Plugin.Sms
{
    public class ProvidersFactory
    {
        private readonly HttpClient _httpClient;

        public ProvidersFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public INotificationProvider GetProvider(AvailableProviders selectedProvider, string apiKey)
        {
            INotificationProvider provider = selectedProvider switch
            {
                AvailableProviders.Clickattel => new ClickatellProvider(_httpClient, apiKey),
                _ => throw new ArgumentOutOfRangeException()
            };

            return provider;
        }
    }
}
