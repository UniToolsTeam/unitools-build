using System;

namespace UniTools.CLI
{
    public sealed class TarAttribute : BaseCliToolAttribute
    {
        private const string ToolName = "tar";

        public TarAttribute() : base(ToolName)
        {
        }

        public override BaseCliTool Create()
        {
            return new Tar(
                PathResolver.Default.Execute(ToolName).Output.Split(Environment.NewLine.ToCharArray())?[0],
                CommandLine.Default);
        }
    }
}