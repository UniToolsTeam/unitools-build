namespace UniTools.Build
{
    public sealed class GenericTool : BaseCliTool
        , ICliToolFriendlyName
    {
        private readonly CommandLine m_commandLine = default;

        // public GenericTool(string executable, CommandLine commandLine)
        public GenericTool(string executable, string path, CommandLine commandLine)
        {
            Name = executable;
            Path = path;
            m_commandLine = commandLine;
        }

        public string Name { get; private set; }

        public override string Path { get; }

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
            return $"{nameof(GenericTool)}: {Path}";
        }
    }
}