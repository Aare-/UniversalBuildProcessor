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
        [EnsureNotPlaceholder]
        [JsonProperty("APPSFLYER_APP_ID", Required = Required.Always)] 
        public string AppsFlyerAppId;
        
        [EnsureNotPlaceholder]
        [JsonProperty("APPSFLYER_DEV_KEY", Required = Required.Always)] 
        public string AppsFlyerDevKey;
    }
}