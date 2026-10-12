using System;

namespace UniTools.Build
{
    public sealed class CliToolAttribute : BaseCliToolAttribute
    {
        public CliToolAttribute(string executable) : base(executable)
        {
        }

        public override BaseCliTool Create()
        {
            string path = PathResolver.ResolveExecutable(Executable);

            return new GenericTool(Executable, path, CommandLine.Default);
        }
    }
}