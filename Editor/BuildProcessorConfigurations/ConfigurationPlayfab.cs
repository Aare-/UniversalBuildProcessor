using System;
using Newtonsoft.Json;
using Plugins.UniversalBuildProcessor.Editor.Attributes;

namespace UniversalBuildProcessor.Editor.BuildProcessorConfiguration
{
    [Serializable]
    public class ConfigurationPlayfab
    {
        //PlayFab
        [DebugConfigFieldAttribute]
        [EnsureNotPlaceholder]
        [JsonProperty("PLAYFAB_TITLE_ID", Required = Required.Always)] 
        public string PlayFabTitleId;
        
        [EnsureNotPlaceholder]
        [JsonProperty("PLAYFAB_DEV_KEY", Required = Required.Always)] 
        public string PlayFabDevKey;
    }
}