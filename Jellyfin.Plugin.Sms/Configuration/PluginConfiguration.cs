using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Sms.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public SmsOptions[] Options { get; set; }
        public PluginConfiguration()
        {
            Options = new SmsOptions[]
            {
            };
        }
    }

    public class SmsOptions
    {
        public bool Enabled { get; set; }
        public Providers Provider { get; set; }
        public string PhoneNumber { get; set; }
        public string ApiKey { get; set; }
        public string UserId { get; set; }

        public SmsOptions()
        {
            Provider = Providers.Clickattel;
        }
    }
}