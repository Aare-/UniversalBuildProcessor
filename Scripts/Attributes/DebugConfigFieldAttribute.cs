using System;

namespace Plugins.UniversalBuildProcessor.Editor.Attributes
{
    /**
     * Add this annotation to config field to print it's value during build
     */
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DebugConfigFieldAttribute : Attribute
    {
        
    }
}