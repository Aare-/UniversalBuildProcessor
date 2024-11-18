using System;
using System.Linq;

namespace UniversalBuildProcessor.Editor.BuildProcessorConfiguration
{
    public class BuildProcessorArgs
    {
        public bool UseSemVer => _UseSemVer;

        public string BuildTarget { get; }
        
        public string BuildEnvironment { get; }
        
        public string OutputPath { get; }
        
        public string BuildName { get; }
        
        private readonly bool _UseSemVer = false;
        
        // Optional, only required when using semantic versioning
        public string BuildVersion { get; }
        
        // Optional, only required when using semantic versioning
        public string RunNumber { get; }
        
        public BuildProcessorArgs()
        {
            var args = Environment.GetCommandLineArgs();

            string GetArgument(string argName, string def = null)
            {
                for (var i = 0; i < args.Length; i++)
                {
                    if (args[i] == $"-{argName}")
                    {
                        if (i + 1 >= args.Length)
                        {
                            throw new UnityEditor.Build.BuildFailedException($"No value provided for -{argName}");
                        }

                        return args[i + 1];
                    }
                }
                
                if (def == null)
                    throw new UnityEditor.Build.BuildFailedException($"Obligatory command line argument {argName} missing");
                
                return def;
            }

            string GetArgumentDebugValue(string argName, string dev = null) 
            {
                var value = GetArgument(argName, def);

                Debug.Log($"{BuildProcessor.TAG} Argument '{argName}' value: '{value}'");

                return value;
            }
            
            BuildTarget = GetArgumentDebugValue("buildTarget");
            BuildName = GetArgumentDebugValue("buildName");
            BuildEnvironment = GetArgumentDebugValue("buildEnvironment");
            OutputPath = GetArgumentDebugValue("outputPath");
            _UseSemVer = args.Any(x => x.StartsWith("-semVer"));

            if (_UseSemVer)
            {
                BuildVersion = GetArgument("buildVersion");
                RunNumber = GetArgument("runNumber");
            }
        }
    }
}