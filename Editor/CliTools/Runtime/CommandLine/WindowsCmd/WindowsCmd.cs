using System;

namespace UniTools.Build
{
    public sealed class WindowsCmd : CommandLine, ICliToolVersion, ICliToolFriendlyName
    {
        private string m_version = string.Empty;
        private readonly string m_path;
        public WindowsCmd()
        {
            m_path = UnityEnvironment.Get("ComSpec")?.Value
                ?? System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");
        }
        public string Version
        {
            get
            {
                if (string.IsNullOrEmpty(m_version)) m_version = Execute("ver").Output.Trim();
                return m_version;
            }
        }
        public override string Path => m_path;
        public string Name => nameof(WindowsCmd);
        public override ToolResult Execute(string arguments = null, string workingDirectory = null)
        {
            if (!IsInstalled) throw new ToolNotInstalledException();
            return RunProcess(Path, "/d /s /c \"" + (arguments ?? string.Empty) + "\"", workingDirectory);
        }
    }
}
