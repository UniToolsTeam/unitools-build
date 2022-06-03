using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build
{
    /// <summary>
    /// Creates iOS archive from the xCode project using CLI tools
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(Archive),
        menuName = MenuPaths.IOS + nameof(Archive)
    )]
    public abstract class Archive : IosPostBuildStep
    {
        [SerializeField] private PathProperty m_projectPath = new PathProperty("Unity-iPhone.xcodeproj");
        [SerializeField] private PathProperty m_outputPath = new PathProperty("Unity-iPhone.xcarchive");
        [SerializeField] private string m_scheme = "Unity-iPhone";
        [SerializeField] private bool m_useModernBuildSystem = true;
        protected abstract string CommandStart { get; }

        public override async Task Execute()
        {
            await Task.CompletedTask;

            XCodeBuild build = Cli.Tool<XCodeBuild>();
            string command =
                $"-{CommandStart} {m_projectPath}" +
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