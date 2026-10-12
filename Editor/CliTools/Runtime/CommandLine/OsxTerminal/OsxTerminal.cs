using System;

namespace UniTools.Build
{
    public sealed class OsxTerminal : CommandLine, ICliToolVersion, ICliToolFriendlyName
    {
        private string m_version = string.Empty;
        public OsxTerminal()
        {
            string shell = UnityEnvironment.Get("SHELL")?.Value;
            Path = !string.IsNullOrWhiteSpace(shell) && System.IO.File.Exists(shell) ? shell : "/bin/sh";
        }
        public string Version
        {
            get
            {
                if (string.IsNullOrEmpty(m_version)) m_version = Execute("echo $0").Output.Trim();
                return m_version;
            }
        }
        public override string Path { get; }
        public string Name => nameof(OsxTerminal);
        public override ToolResult Execute(string arguments = null, string workingDirectory = null)
        {
            if (!IsInstalled) throw new ToolNotInstalledException();
            return RunProcess(Path, "-c " + QuoteProcessArgument(arguments ?? string.Empty), workingDirectory);
        }
    }
}
