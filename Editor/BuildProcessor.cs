using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UniversalBuildProcessor.Editor.BuildProcessorConfiguration;

public partial class BuildProcessor
{
    public const string TAG = "[BuildProcessor]";
    
    private const string PRODUCTION_BUILD_ENVIRONMENT = "production";

    private const string BUILD_TARGET_ANDROID = "android";
    private const string BUILD_TARGET_IOS = "ios";

    private static bool IsProd => _IsProdResolver.Invoke();
    
    private static Func<bool> _IsProdResolver = null;

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

        #region Resolve dependencies
        _IsProdResolver = () => {
            return BuildProcessorArgsInstance.BuildEnvironment == PRODUCTION_BUILD_ENVIRONMENT;
        };
        #endregion
        
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
    
    public static void UpdateConfiguration() {
        Debug.Log($"{TAG} Updating build configuration (is prod: {IsProd}) ...");
        
        AssetDatabase.Refresh();
        
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

    [MenuItem("Tools/UniversalBuildProcessor/Update With Configuration/Prod")]
    public static void UpdateConfigurationProd() {
        #region Resolve dependencies
        _IsProdResolver = () => { return true; };
        #endregion
        
        UpdateConfiguration();
    }
    
    [MenuItem("Tools/UniversalBuildProcessor/Update With Configuration/Qa")]
    public static void UpdateConfigurationQa() {
        #region Resolve dependencies
        _IsProdResolver = () => { return false; };
        #endregion
        
        UpdateConfiguration();
    }
}
