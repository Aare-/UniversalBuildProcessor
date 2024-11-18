using System;
using Newtonsoft.Json;
using Plugins.UniversalBuildProcessor.Editor.Attributes;

namespace UniversalBuildProcessor.Editor.BuildProcessorConfiguration
{
    [Serializable]
    public class ConfigurationAndroid
    {
        //Android
        [DebugConfigFieldAttribute]
        [JsonProperty("ANDROID_APP_NAME", Required = Required.Always)] 
        public string AndroidAppName;
        
        [DebugConfigFieldAttribute]
        [JsonProperty("ANDROID_PACKAGE_NAME", Required = Required.Always)] 
        public string AndroidPackageName;
        
        [JsonProperty("ANDROID_VERSION_CODE", Required = Required.Always)] 
        public int AndroidVersionCode;
        
        [JsonProperty("ANDROID_VERSION_NAME", Required = Required.Always)] 
        public string AndroidVersionName;
        
        [JsonProperty("ANDROID_KEYSTORE_PATH", Required = Required.Always)] 
        public string AndroidKeystorePath;
        
        [EnsureNotPlaceholder]
        [JsonProperty("ANDROID_KEYSTORE_PASSWORD", Required = Required.Always)] 
        public string AndroidKeystorePassword;
        
        [JsonProperty("ANDROID_KEYALIAS", Required = Required.Always)] 
        public string AndroidKeyalias;
        
        [EnsureNotPlaceholder]
        [JsonProperty("ANDROID_KEYALIAS_PASSWORD", Required = Required.Always)] 
        public string AndroidKeyaliasPassword;
    }
}