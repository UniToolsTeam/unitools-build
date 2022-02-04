using System;

namespace UniTools.CLI
{
    public sealed class ZipAttribute : BaseCliToolAttribute
    {
        private const string ToolName = "zip";

        public ZipAttribute() : base(ToolName)
        {
        }

        public override BaseCliTool Create()
        {
            return new Zip(
                PathResolver.Default.Execute(ToolName).Output.Split(Environment.NewLine.ToCharArray())?[0],
                CommandLine.Default);
        }
    }
}