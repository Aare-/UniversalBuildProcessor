using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Facebook.Unity.Editor;
using Facebook.Unity.Settings;
using Plugins.UniversalBuildProcessor.Editor.Attributes;
using Plugins.UniversalBuildProcessor.Editor.BuildProcessorModel;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UniversalBuildProcessor.Editor.BuildProcessorConfiguration;

public partial class BuildProcessor
{
    private static void TryUpdateConfigurationFacebook()
    {
        var (exists, path) = TryGetPathForModelFile<ConfigurationModelFacebook>();
        
        if (!exists)
            return;
        
        var model = (ConfigurationModelFacebook)AssetDatabase.LoadAssetAtPath(
            path, 
            typeof(ConfigurationModelFacebook));
        var config = ConfigurationManagerInstance.GetConfig<ConfigurationFacebook>();
        
        var selectedAppIndex = FacebookSettings.AppIds.IndexOf(config.FacebookAppID);
        
        if (selectedAppIndex < 0)
        {
            throw new UnityEditor.Build.BuildFailedException($"{TAG} Unable to find the configured AppId in FacebookSettings:{config.FacebookAppID}");
        }
        
        FacebookSettings.SelectedAppIndex = selectedAppIndex; 
        ManifestMod.GenerateManifest();
        
        Console.WriteLine(
            $"{TAG} Configured Facebook App Id {FacebookSettings.AppId} at index {FacebookSettings.SelectedAppIndex}");

        model.AppID = config.FacebookAppID;
        
        EditorUtility.SetDirty(model);
        AssetDatabase.SaveAssets();
    }

    private static void TryUpdateConfigurationAmplitude()
    {
        var (exists, path) = TryGetPathForModelFile<ConfigurationModelAmplitude>();
        
        if (!exists)
            return;
        
        var model = (ConfigurationModelAmplitude)AssetDatabase.LoadAssetAtPath(
            path, 
            typeof(ConfigurationModelAmplitude));
        var config = ConfigurationManagerInstance.GetConfig<ConfigurationAmplitude>();

        model.AmplitudeAPIKey = config.AmplitudeAPIKey;
        model.AmplitudeServerZone = config.AmplitudeServerZone;
        model.AmplitudeServerUrl = config.AmplitudeServerUrl;
        
        EditorUtility.SetDirty(model);
        AssetDatabase.SaveAssets();
    }
    
    private static void TryUpdateConfigurationAdmob()
    {
        var (exists, path) = TryGetPathForModelFile<ConfigurationModelAdmob>();
        
        if (!exists)
            return;
        
        var model = (ConfigurationModelAdmob)AssetDatabase.LoadAssetAtPath(
            path, 
            typeof(ConfigurationModelAdmob));
        var config = ConfigurationManagerInstance.GetConfig<ConfigurationAdmob>();

        model.AndroidAdmobAppId = config.AndroidAdmobAppId;
        model.IosAdmobAppId = config.IosAdmobAppId;
        
        model.NoAdCountryCodes = config.NoAdCountryCodes;
        model.TestDeviceId = config.TestAdDeviceId;
        
        EditorUtility.SetDirty(model);
        AssetDatabase.SaveAssets();
    }
    
    private static void TryUpdateConfigurationAppsFlyer()
    {
        var (exists, path) = TryGetPathForModelFile<ConfigurationModelAppsflyer>();
        
        if (!exists)
            return;
        
        var model = (ConfigurationModelAppsflyer)AssetDatabase.LoadAssetAtPath(
            path, 
            typeof(ConfigurationModelAppsflyer));
        var config = ConfigurationManagerInstance.GetConfig<ConfigurationAppsFlyer>();
        
        model.AppId = config.AppsFlyerAppId;
        model.DevKey = config.AppsFlyerDevKey;
        
        EditorUtility.SetDirty(model);
        AssetDatabase.SaveAssets();
    }

    private static void TryUpdateConfigurationGoogle()
    {
        var (exists, path) = TryGetPathForModelFile<ConfigurationModelGoogle>();
        
        if (!exists)
            return;
        
        var model = (ConfigurationModelGoogle)AssetDatabase.LoadAssetAtPath(
            path, 
            typeof(ConfigurationModelGoogle));
        var config = ConfigurationManagerInstance.GetConfig<ConfigurationGoogle>();
        
        model.GoogleWebClientID = config.GoogleWebClientID;

        EditorUtility.SetDirty(model);
        AssetDatabase.SaveAssets();
    }

    private static void TryUpdateConfigurationPlayFab()
    {
        var (exists, path) = TryGetPathForModelFile<ConfigurationModelPlayfab>();
        
        if (!exists)
            return;
        
        var model = (ConfigurationModelPlayfab)AssetDatabase.LoadAssetAtPath(
            path, 
            typeof(ConfigurationModelPlayfab));
        var config = ConfigurationManagerInstance.GetConfig<ConfigurationPlayfab>();
        
        model.TitleId = config.PlayFabTitleId;
        model.DevKey = config.PlayFabDevKey;
        
        EditorUtility.SetDirty(model);
        AssetDatabase.SaveAssets();
    }

    public static void TryUpdateCustomConfiguration()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var ass in assemblies)
        {
            var annotated = ass.GetTypes()
                .Where(t => t.GetCustomAttribute<CustomBuildConfigurationAttribute>() != null)
                .Select(t => (t, t.GetCustomAttribute<CustomBuildConfigurationAttribute>().ConfigurationModelType))
                .ToList();
            
            foreach (var (configType, modelType) in annotated)
            {
                var (exists, path) = TryGetPathForModelFile(modelType);
        
                if (!exists)
                    continue;       
                
                var model = AssetDatabase.LoadAssetAtPath(
                    path, 
                    modelType);
                var config = ConfigurationManagerInstance.GetConfig(configType);
        
                var configFields = config
                    .GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public);
                var modelFields = model
                    .GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public);

                var commonFields = configFields
                    .Select(x => x.Name)
                    .Union(modelFields.Select(x => x.Name))
                    .ToList();

                foreach (var fieldName in commonFields)
                {
                    var valInConfig = configFields.First(x => x.Name == fieldName).GetValue(config);
                    var valInModel = modelFields.First(x => x.Name == fieldName);
                    valInModel.SetValue(model, valInConfig);
                    
                    Debug.Log($"{TAG} - Copied custom field: {fieldName}");
                }
        
                EditorUtility.SetDirty(model);
                AssetDatabase.SaveAssets();
            }
        }
    }
}