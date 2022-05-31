using System.IO;

namespace UniTools.Build
{
    public sealed class CliToolPathAttribute : BaseCliToolAttribute
    {
        private readonly string m_path = default;
        public CliToolPathAttribute(string executable, string path) : base(executable)
        {
            m_path = path;
        }

        public override BaseCliTool Create()
        {
            return new GenericTool(Executable, m_path, CommandLine.Default);
        }
    }
}