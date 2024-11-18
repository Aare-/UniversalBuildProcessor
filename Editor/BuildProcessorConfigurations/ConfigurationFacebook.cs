using System;
using Newtonsoft.Json;
using Plugins.UniversalBuildProcessor.Editor.Attributes;

namespace UniversalBuildProcessor.Editor.BuildProcessorConfiguration
{
    [Serializable]
    public class ConfigurationFacebook
    {
        //Facebook
        [DebugConfigField]
        [JsonProperty("FACEBOOK_APP_ID")] 
        public string FacebookAppID;
    }
}