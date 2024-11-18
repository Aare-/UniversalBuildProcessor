using System;
using Newtonsoft.Json;
using Plugins.UniversalBuildProcessor.Editor.Attributes;

namespace UniversalBuildProcessor.Editor.BuildProcessorConfiguration
{
    [Serializable]
    public class ConfigurationiOS
    {
        [DebugConfigFieldAttribute]
        [JsonProperty("IOS_BUNDLE_ID", Required = Required.Always)] 
        public string IOSBundleID;    
        
        [DebugConfigFieldAttribute]
        [JsonProperty("IOS_SKU", Required = Required.Always)] 
        public string IosSku;
        
        [JsonProperty("IOS_VERSION_CODE", Required = Required.Always)] 
        public string IOSVersionCode;
        
        [JsonProperty("IOS_BUILD_ID", Required = Required.Always)] 
        public string IOSBuildID;
        
        [EnsureNotPlaceholder]
        [JsonProperty("IOS_TEAM_ID", Required = Required.Always)] 
        public string IOSTeamID;
        
        [EnsureNotPlaceholder]
        [JsonProperty("IOS_APPLE_ID", Required = Required.Always)] 
        public string IOSAppleID;
        
        [EnsureNotPlaceholder]
        [JsonProperty("IOS_APP_ID", Required = Required.Always)] 
        public string IOSAppID;
        
        [EnsureNotPlaceholder]
        [JsonProperty("IOS_SIGNING_ENTITY", Required = Required.Always)] 
        public string IOSSigningEntity;
        
        [JsonProperty("IOS_PROVISIONING_PROFILE", Required = Required.Always)] 
        public string IOSProvisioningProfile;
        
        [JsonProperty("IOS_API_KEY", Required = Required.Always)] 
        public string IOSAPIKey;
        
        [DebugConfigFieldAttribute]
        [JsonProperty("IOS_APP_NAME", Required = Required.Always)] 
        public string IOSAppName;
    }
}