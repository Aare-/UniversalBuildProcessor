using UnityEngine;

namespace Plugins.UniversalBuildProcessor.Editor.BuildProcessorModel
{
    [CreateAssetMenu(
        fileName = "PlayfabConfigurationModel", 
        menuName = "BuildConfig/PlayfabConfigurationModel")]
    public class ConfigurationModelPlayfab : ScriptableObject
    {
        public string TitleId;
        
        public string DevKey;
    }
}