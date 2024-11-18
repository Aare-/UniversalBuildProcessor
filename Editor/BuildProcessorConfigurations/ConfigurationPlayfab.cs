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
        [JsonProperty("PLAYFAB_TITLE_ID")] 
        public string PlayFabTitleId;
        
        [EnsureNotPlaceholder]
        [JsonProperty("PLAYFAB_DEV_KEY")] 
        public string PlayFabDevKey;
    }
}