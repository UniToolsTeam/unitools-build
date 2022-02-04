namespace UniTools.Build
{
    public sealed class OsxWhich : PathResolver
        , ICliToolFriendlyName
    {
        public string Name => nameof(OsxWhich);
        public override string Path => "/usr/bin/which";

        public OsxWhich(CommandLine commandLine) : base(commandLine)
        {
        }

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
    }
}