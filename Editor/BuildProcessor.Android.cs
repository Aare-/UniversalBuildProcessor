using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UniversalBuildProcessor.Editor.BuildProcessorConfiguration;

public partial class BuildProcessor
{
    private static void BuildAndroidInternal(ConfigurationAndroid config, bool isProd = false)
    {
        Debug.Log($"{TAG} Building using android internal...");
        PlayerSettings.productName = config.AndroidAppName;
        PlayerSettings.applicationIdentifier = config.AndroidPackageName;
        
        if (BuildProcessorArgsInstance.UseSemVer) 
        {
            PlayerSettings.bundleVersion = BuildProcessorArgsInstance.BuildVersion;
            PlayerSettings.Android.bundleVersionCode = GetVersionCode();            
        } 
        else 
        {            
            // Verify arguments
            if (string.IsNullOrEmpty(config.AndroidVersionName))
            {
                throw new UnityEditor.Build.BuildFailedException($"{TAG} Missing android version name in config json!");
            }
            
            PlayerSettings.bundleVersion = config.AndroidVersionName;
            PlayerSettings.Android.bundleVersionCode = config.AndroidVersionCode;
        }

        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = config.AndroidKeystorePath;
        PlayerSettings.Android.keystorePass = config.AndroidKeystorePassword;
        PlayerSettings.Android.keyaliasName = config.AndroidKeyalias;
        PlayerSettings.Android.keyaliasPass = config.AndroidKeyaliasPassword;

        Debug.Log($"{TAG} Using semantic versioning: {BuildProcessorArgsInstance.UseSemVer}, "+
                  $"bundle version: {PlayerSettings.bundleVersion} " +
                  $"version code: {PlayerSettings.Android.bundleVersionCode}");

        var buildPlayerOptions = new BuildPlayerOptions
        {
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        // Check the environment and configure the build output
        if (isProd)
        {
            // Generate AAB for Production
            EditorUserBuildSettings.buildAppBundle = true; 
            buildPlayerOptions.locationPathName = 
                Path.Join(BuildProcessorArgsInstance.OutputPath, $"{BuildProcessorArgsInstance.BuildName}.aab");
        }
        else
        {
            // Generate APK for QA
            EditorUserBuildSettings.buildAppBundle = false; 
            buildPlayerOptions.locationPathName = 
                Path.Join(BuildProcessorArgsInstance.OutputPath, $"{BuildProcessorArgsInstance.BuildName}.apk");
        }
        
        PerformBuild(buildPlayerOptions);
        
        int GetVersionCode() 
        {
            if (string.IsNullOrEmpty(BuildProcessorArgsInstance.RunNumber)) 
            {
                throw new UnityEditor.Build.BuildFailedException($"{TAG} Could not get iOS build number, missing run number argument");
            }

            return 1000000 + Int32.Parse(BuildProcessorArgsInstance.RunNumber);
        }
    }
}