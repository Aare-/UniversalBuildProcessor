using UnityEngine;

namespace Plugins.UniversalBuildProcessor.Editor.BuildProcessorModel
{
    [CreateAssetMenu(
        fileName = "AdmobConfigurationModel", 
        menuName = "BuildConfig/AdmobConfigurationModel")]
    public class ConfigurationModelAdmob : ScriptableObject
    {
        public string AndroidAdmobAppId;
        
        public string IosAdmobAppId;
        
        public string[] NoAdCountryCodes;
        
        public string[] TestDeviceId;
    }
}