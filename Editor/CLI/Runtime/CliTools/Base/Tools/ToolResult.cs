using System.Text;

namespace UniTools.CLI
{
    public sealed class ToolResult
    {
        public readonly string Output;
        public readonly string Error;
        public readonly int ExitCode;

        public ToolResult(string output, string error, int exitCode)
        {
            Output = output;
            Error = error;
            ExitCode = exitCode;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{nameof(ExitCode)}={ExitCode}");
            if (!string.IsNullOrEmpty(Error))
            {
                builder.AppendLine($"{nameof(Error)}={Error}");
            }

            builder.AppendLine($"{nameof(Output)}={Output}");

            return builder.ToString();
        }
    }
}