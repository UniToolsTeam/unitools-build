namespace UniTools.Build
{
    public abstract class CommandLine : BaseCliTool
    {
        public static readonly CommandLine Default =
#if UNITY_EDITOR_OSX
            new OsxTerminal();

#elif UNITY_EDITOR_WIN
            new WindowsCmd();
#else
//TODO Create Unsupported tool!
        default;
#endif
    }
}