using System;

namespace UniTools.Build
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
                PathResolver.ResolveExecutable(ToolName),
                CommandLine.Default);
        }
    }
}