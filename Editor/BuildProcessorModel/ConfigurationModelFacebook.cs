using UnityEngine;

namespace Plugins.UniversalBuildProcessor.Editor.BuildProcessorModel
{
    [CreateAssetMenu(
        fileName = "FacebookConfigurationModel", 
        menuName = "BuildConfig/FacebookConfigurationModel")]
    public class ConfigurationModelFacebook : ScriptableObject
    {
        public string AppID;
    }
}