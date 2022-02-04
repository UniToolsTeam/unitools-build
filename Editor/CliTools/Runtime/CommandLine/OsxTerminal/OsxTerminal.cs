using System;
using System.Diagnostics;
using System.Text;
using Debug = UnityEngine.Debug;

namespace UniTools.Build
{
    public sealed class OsxTerminal : CommandLine
        , ICliToolVersion
        , ICliToolFriendlyName
    {
        private string m_version = string.Empty;

        public OsxTerminal()
        {
            UnityEnvironmentVariableModel shell = UnityEnvironment.Get("SHELL");
            if (shell != null)
            {
                Path = shell.Value;
            }
            else
            {
                Debug.LogWarning($"{nameof(OsxTerminal)}: failed to find SHELL path!");
            }
        }

        public string Version
        {
            get
            {
                if (!IsInstalled)
                {
                    throw new Exception($"{nameof(OsxTerminal)}: Failed to get version due to tool is not installed!");
                }

                if (string.IsNullOrEmpty(m_version))
                {
                    m_version = Execute("--version").Output;
                }

                return m_version;
            }
        }

        public override string Path { get; }

        public string Name => nameof(OsxTerminal);

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

            string escapedArgs = arguments.Replace("\"", "\\\"");
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = workingDirectory,
                    FileName = Path,
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    StandardErrorEncoding = Encoding.UTF8,
                    StandardOutputEncoding = Encoding.UTF8
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();
            int exitCode = process.ExitCode;

            return new ToolResult(
                output,
                error,
                exitCode);
        }
    }
}