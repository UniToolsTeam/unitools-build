using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class Archive : BuildStep
    {
        [SerializeField] private PathProperty m_projectPath = new PathProperty("Unity-iPhone.xcodeproj");
        [SerializeField] private PathProperty m_outputPath = new PathProperty("Unity-iPhone.xcarchive");
        [SerializeField] private string m_scheme = "Unity-iPhone";
        [SerializeField] private string m_destination = "generic/platform=iOS";
        [SerializeField] private string m_teamId = string.Empty;
        [SerializeField] private bool m_useModernBuildSystem = true;
        [SerializeField] private bool m_enableBitcode = false;
        [SerializeField] private bool m_overrideProvisioningProfile = false;
        [SerializeField] private string m_provisioningProfileUuid = string.Empty;
        [SerializeField, Tooltip("Arguments that needs to be added, but not in the list above")] private string m_extraArgs = string.Empty;

        protected abstract string CommandStart { get; }

        public override async Task Execute()
        {
            await Task.CompletedTask;

            XCodeBuild build = Cli.Tool<XCodeBuild>();
            if (build == null || !build.IsInstalled) throw new ToolNotInstalledException();
            string command =
                " archive" +
                $" -{CommandStart} {CommandLine.QuoteArgument(PathProperty.RequirePath(m_projectPath, "Xcode project"))}" +
                $" -scheme {CommandLine.QuoteArgument(m_scheme)}" +
                $" -archivePath {CommandLine.QuoteArgument(PathProperty.RequirePath(m_outputPath, "Archive output"))}";

            if (!string.IsNullOrWhiteSpace(m_destination))
            {
                command += $" -destination {CommandLine.QuoteArgument(m_destination)}";
            }

            if (!string.IsNullOrWhiteSpace(m_extraArgs))
            {
                command += $" {m_extraArgs} ";
            }

            command += $" {CommandLine.QuoteArgument("DEVELOPMENT_TEAM=" + m_teamId)}";

            if (m_overrideProvisioningProfile)
            {
                command += $" {CommandLine.QuoteArgument("PROVISIONING_PROFILE=" + m_provisioningProfileUuid)}";
            }

            if (m_enableBitcode)
            {
                command += " ENABLE_BITCODE=YES";
            }
            else
            {
                command += " ENABLE_BITCODE=NO";
            }

            if (m_useModernBuildSystem)
            {
                command += " -UseModernBuildSystem=YES";
            }
            else
            {
                command += " -UseModernBuildSystem=NO";
            }

            ToolResult result = build.Execute(command, ProjectPath.Value);
            if (result.ExitCode != 0)
            {
                throw new BuildStepFailedException($"{nameof(Archive)}: Failed! {result.ToString()}");
            }
        }
    }
}
