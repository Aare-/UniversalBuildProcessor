using System;
using Newtonsoft.Json;
using Plugins.UniversalBuildProcessor.Editor.Attributes;

namespace UniversalBuildProcessor.Editor.BuildProcessorConfiguration
{
    [Serializable]
    public class ConfigurationGoogle
    {
        //Google
        [JsonProperty("GOOGLE_WEB_CLIENT_ID")] 
        public string GoogleWebClientID;
    }
}