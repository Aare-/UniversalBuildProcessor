using UnityEngine;

namespace Plugins.UniversalBuildProcessor.Editor.BuildProcessorModel
{
    [CreateAssetMenu(
        fileName = "GoogleConfigurationModel", 
        menuName = "BuildConfig/GoogleConfigurationModel")]
    public class ConfigurationModelGoogle : ScriptableObject
    {
        public string GoogleWebClientID;
    }
}