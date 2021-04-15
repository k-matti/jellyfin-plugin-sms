using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Sms.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public SmsOptions[] Options { get; set; }
        public PluginConfiguration()
        {
            Options = System.Array.Empty<SmsOptions>();
        }
    }

    public class SmsOptions
    {
        public bool Enabled { get; set; }
        public AvailableProviders Provider { get; set; }
        public string PhoneNumber { get; set; }
        public string ApiKey { get; set; }
        public string UserId { get; set; }

        public SmsOptions()
        {
            Provider = AvailableProviders.Clickattel;
        }
    }
}
