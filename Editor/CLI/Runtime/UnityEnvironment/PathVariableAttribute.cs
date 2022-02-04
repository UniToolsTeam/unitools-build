using System;

//NOTE Example of usage
// [assembly: PathVariable("/usr/bin")]
// [assembly: PathVariable("/usr/local/share/dotnet")]

namespace UniTools.CLI
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class PathVariableAttribute : Attribute
    {
        public readonly string Value;

        public PathVariableAttribute(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"{nameof(PathVariableAttribute)}: {Value}";
        }
    }
}