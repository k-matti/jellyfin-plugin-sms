using System.Net.Http;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.Sms.Providers
{
    public interface INotificationProvider
    {
        Task<HttpResponseMessage> SendMessage(string phoneNumber, string content);
    }
}
