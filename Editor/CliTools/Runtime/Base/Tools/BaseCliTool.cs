namespace UniTools.Build
{
    public abstract class BaseCliTool :
        ICliToolPath
        , ICliToolInstalled
    {
        public bool IsInstalled
        {
            get
            {
                try
                {
                    return System.IO.Path.IsPathRooted(Path);
                }
                catch
                {
                    return false;
                }
            }
        }

        public abstract string Path { get; }
        public abstract ToolResult Execute(string arguments = null, string workingDirectory = null);
    }
}