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
                PathResolver.Default.Execute(XCodeBuild.ToolName).Output.Replace(Environment.NewLine, string.Empty),
                CommandLine.Default);
        }
    }
}