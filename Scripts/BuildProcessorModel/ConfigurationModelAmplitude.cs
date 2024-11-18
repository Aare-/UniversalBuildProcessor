using UnityEngine;

namespace Plugins.UniversalBuildProcessor.Editor.BuildProcessorModel
{
    [CreateAssetMenu(
        fileName = "AmplitudeConfigurationModel", 
        menuName = "BuildConfig/AmplitudeConfigurationModel")]
    public class ConfigurationModelAmplitude : ScriptableObject
    {
        public string AmplitudeAPIKey;
        
        public string AmplitudeServerZone;
        
        public string AmplitudeServerUrl;
    }
}