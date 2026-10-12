using System;

namespace UniTools.Build
{
    public sealed class XCodeBuildAttribute : BaseCliToolAttribute
    {
        public XCodeBuildAttribute() : base(XCodeBuild.ToolName)
        {
        }

        public override BaseCliTool Create()
        {
            return new XCodeBuild(
                PathResolver.ResolveExecutable(XCodeBuild.ToolName),
                CommandLine.Default);
        }
    }
}