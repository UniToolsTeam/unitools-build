using UniTools.Build;

[assembly: Zip]

namespace UniTools.Build
{
    public sealed class Zip : BaseCliTool
        , ICliToolFriendlyName
    {
        private readonly CommandLine m_commandLine = default;

        public override string Path { get; } = default;

        public string Name => $"{nameof(Zip)} (archive tool)";

        public Zip(string path, CommandLine commandLine)
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