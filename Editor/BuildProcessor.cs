using System.IO;
using UnityEngine;
using UniversalBuildProcessor.Editor.BuildProcessorConfiguration;

public partial class BuildProcessor
{
    public const string TAG = "[BuildProcessor]";
    
    private const string PRODUCTION_BUILD_ENVIRONMENT = "production";

    private const string BUILD_TARGET_ANDROID = "android";
    private const string BUILD_TARGET_IOS = "ios";
    
    private static bool IsProd => BuildProcessorArgsInstance.BuildEnvironment == PRODUCTION_BUILD_ENVIRONMENT;

    private static BuildProcessorArgs BuildProcessorArgsInstance
    {
        get
        {
            _BuildProcessorArgsInstance ??= new BuildProcessorArgs();
            return _BuildProcessorArgsInstance;
        }
    }
    
    private static ConfigurationManager ConfigurationManagerInstance
    {
        get
        {
            _ConfigurationManagerInstance ??= new ConfigurationManager(IsProd, false, ConfigPath);
            return _ConfigurationManagerInstance;
        }
    }
    
    private static string ConfigPath(bool isProd)
    {
        var configFileName = QA_CONFIG_JSON_NAME;
        
        if (isProd)
        {
            configFileName = PROD_CONFIG_JSON_NAME;
        }

        return Path.Join(CONFIG_PATH_FOLDER, configFileName);
    }
    
    private static BuildProcessorArgs _BuildProcessorArgsInstance;
    
    private static ConfigurationManager _ConfigurationManagerInstance;

    public static void Build()
    {
        Debug.Log($"{TAG} Starting build processor ...");

        UpdateConfiguration();

        switch (BuildProcessorArgsInstance.BuildTarget.ToLowerInvariant())
        {
            case BUILD_TARGET_ANDROID:
                BuildAndroidInternal(ConfigurationManagerInstance.GetConfig<ConfigurationAndroid>(), IsProd);
                break;
            
            case BUILD_TARGET_IOS:
                BuildIOSInternal(ConfigurationManagerInstance.GetConfig<ConfigurationiOS>());
                break;
        }

        _BuildProcessorArgsInstance = null;
    }

    [MenuItem("Tools/UniversalBuildProcessor/Update Configuration")]
    public static void UpdateConfiguration() {
        Debug.Log($"{TAG} Updating build configuration ...");
        
        // Built-in optional configuration
        TryUpdateConfigurationAmplitude();
        TryUpdateConfigurationAdmob();
        TryUpdateConfigurationGoogle();
        TryUpdateConfigurationAppsFlyer();
        TryUpdateConfigurationFacebook();
        TryUpdateConfigurationPlayFab();
        
        // Custom configuration
        TryUpdateCustomConfiguration();
    }
}
