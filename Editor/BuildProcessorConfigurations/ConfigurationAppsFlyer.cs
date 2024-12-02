using System;
using Newtonsoft.Json;
using Plugins.UniversalBuildProcessor.Editor.Attributes;

namespace UniversalBuildProcessor.Editor.BuildProcessorConfiguration
{
    [Serializable]
    public class ConfigurationAppsFlyer
    {
        //AppsFlyer
        [DebugConfigFieldAttribute]
        [JsonProperty("APPSFLYER_APP_ID")] 
        public string AppsFlyerAppId;
        
        [JsonProperty("APPSFLYER_DEV_KEY")] 
        public string AppsFlyerDevKey;
    }
}