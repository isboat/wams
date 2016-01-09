using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Wams.Common.Configuration
{
    using System.Configuration;

    public class UpaCarConfiguration : ConfigurationSection
    {
        private static readonly UpaCarConfiguration Section = ConfigurationManager.GetSection("upacarConfig") as UpaCarConfiguration;

        [ConfigurationProperty("desktopWebClientId", IsRequired = true)]
        protected string DesktopWebClientIdSetting
        {
            get { return (string)this["desktopWebClientId"]; }
        }

        [ConfigurationProperty("iosClientId", IsRequired = true)]
        protected string IosClientIdSetting
        {
            get { return (string)this["iosClientId"]; }
        }

        [ConfigurationProperty("androidClientId", IsRequired = true)]
        protected string AndroidClientIdSetting
        {
            get { return (string)this["androidClientId"]; }
        }

        [ConfigurationProperty("windowsClientId", IsRequired = true)]
        protected string WindowsClientIdSetting
        {
            get { return (string)this["windowsClientId"]; }
        }

        public string DesktopWebClientId
        {
            get { return Section.DesktopWebClientIdSetting; }
        }

        public string IosClientId
        {
            get { return Section.IosClientIdSetting; }
        }

        public string AndroidClientId
        {
            get { return Section.AndroidClientIdSetting; }
        }

        public string WindowsClientId
        {
            get { return Section.WindowsClientIdSetting; }
        }

        public bool IsClientId(string clientId)
        {
            return this.DesktopWebClientId == clientId 
                || this.IosClientId == clientId
                || this.AndroidClientId == clientId
                || this.WindowsClientId == clientId;
        }
    }
}
