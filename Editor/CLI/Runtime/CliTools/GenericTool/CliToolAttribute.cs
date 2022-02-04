using System;

namespace UniTools.CLI
{
    public sealed class CliToolAttribute : BaseCliToolAttribute
    {
        public CliToolAttribute(string executable) : base(executable)
        {
        }

        public override BaseCliTool Create()
        {
            string path = PathResolver.Default.Execute(Executable).Output.Replace(Environment.NewLine, string.Empty);

            return new GenericTool(Executable, path, CommandLine.Default);
        }
    }
}