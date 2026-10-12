namespace UniTools.Build
{
    public sealed class WindowsWhere : PathResolver
        , ICliToolFriendlyName
    {
        public string Name => nameof(WindowsWhere);
        public override string Path => System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System), "where.exe");

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

            return CommandLine.Execute($"{Path} {arguments}", workingDirectory);
        }

        public WindowsWhere(CommandLine commandLine) : base(commandLine)
        {
        }
    }
}