using System;

namespace UniTools.CLI
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class BaseCliToolAttribute : Attribute
    {
        public readonly string Executable = default;

        protected BaseCliToolAttribute(string executable)
        {
            Executable = executable;
        }

        public override string ToString()
        {
            return $"{nameof(Executable)}={Executable}";
        }

        public abstract BaseCliTool Create();
    }
}