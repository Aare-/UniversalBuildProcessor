using System;
using Newtonsoft.Json;
using Plugins.UniversalBuildProcessor.Editor.Attributes;

namespace UniversalBuildProcessor.Editor.BuildProcessorConfiguration
{
    [Serializable]
    public class ConfigurationGoogle
    {
        //Google
        [EnsureNotPlaceholder]
        [JsonProperty("GOOGLE_WEB_CLIENT_ID", Required = Required.Always)] 
        public string GoogleWebClientID;
    }
}