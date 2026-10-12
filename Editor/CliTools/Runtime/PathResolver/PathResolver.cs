namespace UniTools.Build
{
    public abstract class PathResolver : BaseCliTool
    {
        public static readonly PathResolver Default =
#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            new OsxWhich(CommandLine.Default);

#elif UNITY_EDITOR_WIN
            new WindowsWhere(CommandLine.Default);
#else
//TODO Create Unsupported tool!
        default;
#endif

        internal static string ResolveExecutable(string executable)
        {
            if (Default == null) throw new System.PlatformNotSupportedException("No command-line path resolver for this Editor.");
            ToolResult result = Default.Execute(UniTools.Build.CommandLine.QuoteArgument(executable));
            if (result.ExitCode != 0) return string.Empty;
            foreach (string line in result.Output.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries))
            {
                string path = line.Trim();
                if (System.IO.File.Exists(path)) return path;
            }
            return string.Empty;
        }

        protected readonly CommandLine CommandLine = default;

        protected PathResolver(CommandLine commandLine)
        {
            CommandLine = commandLine;
        }
    }
}