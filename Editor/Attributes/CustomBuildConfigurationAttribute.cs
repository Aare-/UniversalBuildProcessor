using System;

namespace Plugins.UniversalBuildProcessor.Editor.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomBuildConfigurationAttribute : Attribute
    {
        public Type ConfigurationModelType;

        public CustomBuildConfigurationAttribute(Type configurationModelType)
        {
            ConfigurationModelType = configurationModelType;
        }
    }
}