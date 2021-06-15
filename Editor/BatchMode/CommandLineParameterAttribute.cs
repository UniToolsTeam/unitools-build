using System;

namespace UniTools.Build
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class CommandLineParameterAttribute : Attribute
    {
    }
}