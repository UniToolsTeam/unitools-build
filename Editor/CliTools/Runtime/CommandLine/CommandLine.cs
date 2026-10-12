using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace UniTools.Build
{
    public abstract class CommandLine : BaseCliTool
    {
        public static readonly CommandLine Default =
#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            new OsxTerminal();
#elif UNITY_EDITOR_WIN
            new WindowsCmd();
#else
            null;
#endif

        // Quote one literal shell argument. Execute still accepts an intentional shell command.
        public static string QuoteArgument(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
#if UNITY_EDITOR_WIN
            if (value.IndexOf('"') >= 0 || value.IndexOf('\r') >= 0 || value.IndexOf('\n') >= 0)
                throw new ArgumentException("A cmd argument cannot contain quotes or newlines.", nameof(value));
            return "\"" + value + "\"";
#else
            return "'" + value.Replace("'", "'\"'\"'") + "'";
#endif
        }

        protected static ToolResult RunProcess(string executable, string arguments, string workingDirectory)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = executable,
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory ?? string.Empty,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };
                process.Start();
                // Drain both pipes concurrently: sequential reads deadlock on a full stderr pipe.
                Task<string> output = process.StandardOutput.ReadToEndAsync();
                Task<string> error = process.StandardError.ReadToEndAsync();
                process.WaitForExit();
                return new ToolResult(output.GetAwaiter().GetResult(), error.GetAwaiter().GetResult(), process.ExitCode);
            }
        }

        protected static string QuoteProcessArgument(string value)
        {
            // ProcessStartInfo.Arguments uses C-style quoting, independently of shell quoting.
            var builder = new StringBuilder("\"");
            int slashes = 0;
            foreach (char character in value)
            {
                if (character == '\\') { slashes++; continue; }
                if (character == '"') builder.Append('\\', slashes * 2 + 1);
                else builder.Append('\\', slashes);
                builder.Append(character);
                slashes = 0;
            }
            return builder.Append('\\', slashes * 2).Append('"').ToString();
        }
    }
}
