using System;

namespace Plugins.UniversalBuildProcessor.Editor.Attributes
{
    /**
     * Add this annotation to config field to ensure it's value is different in prod from qa
     */
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EnsureNotPlaceholderAttribute : Attribute
    {
        
    }
}