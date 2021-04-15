using System.Net.Http;

namespace Jellyfin.Plugin.Sms.Providers
{
    public interface INotificationProvider
    {
        HttpRequestMessage CreateHttpRequestMessage();
    }
}
