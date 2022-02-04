using UnityEngine;

#if UNITY_IOS
using UniTools.CLI;
[assembly: XCodeBuild]
#endif

namespace UniTools.CLI
{
    public sealed class XCodeBuild : BaseCliTool
        , ICliToolVersion
        , ICliToolFriendlyName
    {
        public const string ToolName = "xcodebuild";
        private readonly CommandLine m_commandLine = default;
        private string m_version = string.Empty;

        public XCodeBuild(string path, CommandLine commandLine)
        {
            Path = path;
            m_commandLine = commandLine;
        }

        public string Name => nameof(XCodeBuild);

        public string Version
        {
            get
            {
                if (string.IsNullOrEmpty(m_version))
                {
                    ToolResult r = Execute("-version");
                    if (r.ExitCode == 0)
                    {
                        m_version = r.Output.Replace("\n", " ");
                    }
                    else
                    {
                        Debug.LogError($"{nameof(XCodeBuild)}: ExitCode {r.ExitCode}. Failed to get version due to {r.Error}");
                    }
                }

                return m_version;
            }
        }

        public override string Path { get; } = default;

        public override ToolResult Execute(string arguments = null, string workingDirectory = null)
        {
            if (string.IsNullOrEmpty(arguments))
            {
                arguments = string.Empty;
            }

            if (string.IsNullOrEmpty(workingDirectory))
            {
                workingDirectory = string.Empty;
            }

            return m_commandLine.Execute($"{Path} {arguments}", workingDirectory);
        }

        public override string ToString()
        {
            return $"{nameof(XCodeBuild)}: {Path}, {Version}";
        }
    }
}