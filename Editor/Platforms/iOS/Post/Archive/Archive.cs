using System.Threading.Tasks;
using UniTools.CLI;
using UniTools.IO;
using UnityEngine;

namespace UniTools.Build.iOS
{
    /// <summary>
    /// Creates iOS archive from the xCode project using CLI tools
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(Archive),
        menuName = nameof(UniTools) + "/Build/Steps/" + nameof(iOS) + "/Post/" + nameof(Archive)
    )]
    public sealed class Archive : IosPostBuildStep
    {
        [SerializeField] private PathProperty m_projectPath = new PathProperty("Unity-iPhone.xcodeproj");
        [SerializeField] private PathProperty m_outputPath = new PathProperty("Unity-iPhone.xcarchive");
        [SerializeField] private string m_scheme = "Unity-iPhone";
        [SerializeField] private bool m_useModernBuildSystem = true;

        public override async Task Execute(string pathToBuiltProject)
        {
            await Task.CompletedTask;

            XCodeBuild build = Cli.Tool<XCodeBuild>();
            string command =
                $"-project {m_projectPath}" +
                $" -scheme \"{m_scheme}\"" +
                " archive" +
                $" -archivePath {m_outputPath}" +
                $" DEVELOPMENT_TEAM={TeamId}" +
                $" PROVISIONING_PROFILE={ProvisioningProfileUuid}";

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
                throw new PostBuildStepFailedException($"{nameof(Archive)}: Failed! {result.ToString()}");
            }
        }
    }
}