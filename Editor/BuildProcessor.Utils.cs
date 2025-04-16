using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public partial class BuildProcessor
{
    private static void PerformBuild(BuildPlayerOptions options)
    {
        options.scenes = EditorBuildSettings
            .scenes
            .Select(x => x.path)
            .ToArray(); 
            
        var report = BuildPipeline.BuildPlayer(options);
        var summary = report.summary;
        
        switch (summary.result)
        {
            case BuildResult.Succeeded:
                Debug.Log($"{TAG} Build succeeded: {summary.totalSize} bytes");
                break;
            
            case BuildResult.Failed:
                throw new UnityEditor.Build.BuildFailedException($"{TAG} Build failed");
        }
    }

    private static (bool, string) TryGetPathForModelFile<T>()
    {
        return TryGetPathForModelFile(typeof(T));
    }

    private static (bool, string) TryGetPathForModelFile(Type t)
    {
        var attribute = (CreateAssetMenuAttribute)Attribute.GetCustomAttribute(
            t, 
            typeof(CreateAssetMenuAttribute));
        var modelPath = Path.Join(
            CONFIG_MODEL_PATH_FOLDER,
            $"{attribute.fileName}.asset");

        if (!File.Exists(modelPath))
        {
            throw new UnityEditor.Build.BuildFailedException($"{TAG} Model not found at: {modelPath} - did you missplace it?");
        }

        Debug.Log($"{TAG} Found Config Model: {attribute.fileName}");
        return (true, modelPath);
    }
}
