using System;
using Newtonsoft.Json;
using Plugins.UniversalBuildProcessor.Editor.Attributes;

namespace UniversalBuildProcessor.Editor.BuildProcessorConfiguration
{
    [Serializable]
    public class ConfigurationAdmob
    {
        [DebugConfigFieldAttribute]
        [JsonProperty("ANDROID_ADMOB_APP_ID")] 
        public string AndroidAdmobAppId;
        
        [DebugConfigFieldAttribute]
        [JsonProperty("IOS_ADMOB_APP_ID")] 
        public string IosAdmobAppId;
        
        [JsonProperty("NO_AD_COUNTRY_CODES")] 
        public string[] NoAdCountryCodes;
        
        [JsonProperty("TEST_AD_DEVICE_ID")] 
        public string[] TestAdDeviceId;
    }
}