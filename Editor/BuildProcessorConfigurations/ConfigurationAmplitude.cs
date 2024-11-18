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
        [JsonProperty("AMPLITUDE_API_KEY", Required = Required.Always)] 
        public string AmplitudeAPIKey;
        
        [JsonProperty("AMPLITUDE_SERVER_TIMEZONE", Required = Required.Always)] 
        public string AmplitudeServerZone;
        
        [JsonProperty("AMPLITUDE_SERVER_URL", Required = Required.Always)] 
        public string AmplitudeServerUrl;
    }
}