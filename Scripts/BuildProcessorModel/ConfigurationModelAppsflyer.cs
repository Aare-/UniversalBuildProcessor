using UnityEngine;

namespace Plugins.UniversalBuildProcessor.Editor.BuildProcessorModel
{
    [CreateAssetMenu(
        fileName = "AppsflyerConfigurationModel", 
        menuName = "BuildConfig/AppsflyerConfigurationModel")]
    public class ConfigurationModelAppsflyer : ScriptableObject
    {
        public string AppId;
        
        public string DevKey;
    }
}