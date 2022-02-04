using UniTools.Build;

[assembly: Tar]

namespace UniTools.Build
{
    public sealed class Tar : BaseCliTool
        , ICliToolVersion
        , ICliToolFriendlyName
    {
        private readonly CommandLine m_commandLine = default;
        private string m_version = string.Empty;

        public override string Path { get; } = default;

        public string Version
        {
            get
            {
                if (string.IsNullOrEmpty(m_version))
                {
                    m_version = Execute("--version").Output;
                }

                return m_version;
            }
        }

        public string Name => $"{nameof(Tar)} (archive tool)";

        public Tar(string path, CommandLine commandLine)
        {
            Path = path;
            m_commandLine = commandLine;
        }

        public override ToolResult Execute(string arguments = null, string workingDirectory = null)
        {
            if (!IsInstalled)
            {
                throw new ToolNotInstalledException();
            }

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
    }
}