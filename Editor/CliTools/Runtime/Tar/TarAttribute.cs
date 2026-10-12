using System;

namespace UniTools.Build
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
                PathResolver.ResolveExecutable(ToolName),
                CommandLine.Default);
        }
    }
}