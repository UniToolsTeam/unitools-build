using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class Archive : IosBuildStep
    {
        [SerializeField] private PathProperty m_projectPath = new PathProperty("Unity-iPhone.xcodeproj");
        [SerializeField] private PathProperty m_outputPath = new PathProperty("Unity-iPhone.xcarchive");
        [SerializeField] private string m_scheme = "Unity-iPhone";
        [SerializeField] private bool m_useModernBuildSystem = true;
        [SerializeField] private bool m_enableBitcode = true;
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
            
            if (m_enableBitcode)
            {
                command += " -configuration Release ENABLE_BITCODE=YES";
            }
            else
            {
                command += " -configuration Release ENABLE_BITCODE=NO";
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