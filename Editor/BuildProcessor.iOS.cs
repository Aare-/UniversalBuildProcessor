using System.IO;
using UnityEditor;
using UnityEngine;
using UniversalBuildProcessor.Editor.BuildProcessorConfiguration;

public partial class BuildProcessor
{
    private static void BuildIOSInternal(ConfigurationiOS config)
    {
        Debug.Log($"{TAG} Building using iOS internal...");
        PlayerSettings.productName = config.IOSAppName;
        PlayerSettings.applicationIdentifier = config.IOSBundleID;
        PlayerSettings.iOS.applicationDisplayName = config.IOSAppName;

        if (BuildProcessorArgsInstance.UseSemVer) 
        {
            PlayerSettings.bundleVersion = BuildProcessorArgsInstance.BuildVersion;
            PlayerSettings.iOS.buildNumber = GetBuildNumber();        
        } 
        else 
        {
            PlayerSettings.bundleVersion = config.IOSVersionCode;
            PlayerSettings.iOS.buildNumber = config.IOSBuildID;
        }
        
        Debug.Log(
            $"{TAG} Using semantic versioning: {BuildProcessorArgsInstance.UseSemVer}, " +
            $"bundle version: {PlayerSettings.bundleVersion} " + 
            $"build number: {PlayerSettings.iOS.buildNumber}");

        PlayerSettings.iOS.appleDeveloperTeamID = config.IOSTeamID;
        PlayerSettings.iOS.appleEnableAutomaticSigning = false;
        PlayerSettings.iOS.iOSManualProvisioningProfileID = config.IOSProvisioningProfile;

        var buildPlayerOptions = new BuildPlayerOptions
        {
            locationPathName = BuildProcessorArgsInstance.OutputPath,
            target = BuildTarget.iOS,
            options = BuildOptions.None
        };
        
        PerformBuild(buildPlayerOptions);
        InstallPods();
        
        void InstallPods() 
        {
            Debug.Log($"{TAG} Installing pods...");
        
            string projectPath = Path.Combine(Directory.GetCurrentDirectory(), BuildProcessorArgsInstance.OutputPath);

            var process = new System.Diagnostics.Process();

            process.StartInfo.FileName = "/bin/bash";       
            process.StartInfo.Arguments = $"-c \"cd {projectPath}; pod install\""; 

            process.Start();
            process.WaitForExit();
        }

        string GetBuildNumber()
        {
            var buildVersion = BuildProcessorArgsInstance.BuildVersion;
            var runNumber = BuildProcessorArgsInstance.RunNumber;
        
            if (string.IsNullOrEmpty(buildVersion)) 
            {
                throw new UnityEditor.Build.BuildFailedException($"{TAG} Could not get iOS build number, missing build version argument");
            }

            if (string.IsNullOrEmpty(runNumber)) 
            {
                throw new UnityEditor.Build.BuildFailedException($"{TAG} Could not get iOS build number, missing run number argument");
            }

            var split = buildVersion.Split(".");
            var joined = string.Join(".", split[0], split[1]);
            joined = string.Join(".", joined, runNumber);

            Debug.Log($"{TAG} Getting iOS build number as: {joined}");

            return joined;
        }
    }
}