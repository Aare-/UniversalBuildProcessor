using System;
using Newtonsoft.Json;
using Plugins.UniversalBuildProcessor.Editor.Attributes;

namespace UniversalBuildProcessor.Editor.BuildProcessorConfiguration
{
    [Serializable]
    public class ConfigurationAmplitude
    {
        //Amplitude
        [EnsureNotPlaceholder]
        [DebugConfigFieldAttribute]
        [JsonProperty("AMPLITUDE_API_KEY", Required = Required.Always)] 
        public string AmplitudeAPIKey;
        
        [DebugConfigFieldAttribute]
        [JsonProperty("AMPLITUDE_SERVER_TIMEZONE")]         
        public string AmplitudeServerZone;
        
        [DebugConfigFieldAttribute]
        [JsonProperty("AMPLITUDE_SERVER_URL")] 
        public string AmplitudeServerUrl;
    }
}