#if UNITY_IOS
using UniTools.CLI;
using System.IO;
using UnityEditor.iOS.Xcode;
#else
using System;
#endif
using System.Threading.Tasks;
using UniTools.IO;
using UnityEngine;

namespace UniTools.Build.iOS
{
    [CreateAssetMenu(
        fileName = nameof(UploadToAppStore),
        menuName = nameof(UniTools) + "/Build/Steps/" + nameof(iOS) + "/Post/" + nameof(UploadToAppStore)
    )]
    public sealed class UploadToAppStore : DistributeIosApplicationStep
    {
        [SerializeField] private PathProperty m_pathToXCodeProject = default;
        [SerializeField] private PathProperty m_archivePath = new PathProperty("Unity-iPhone.xcarchive");
        [SerializeField] private PathProperty m_outputPath = default;

        public override async Task Execute()
        {
            await Task.CompletedTask;

#if UNITY_IOS
            PlistDocument exportOptions = CreateExportOptions();
            string exportOptionsPath = ExportOptionsPath(m_pathToXCodeProject.ToString());

            exportOptions.root.SetString("method", "app-store");
            exportOptions.root.SetString("destination", "upload");

            File.WriteAllText(exportOptionsPath, exportOptions.WriteToString());

            XCodeBuild build = Cli.Tool<XCodeBuild>();

            string command =
                $"-exportArchive " +
                $" -archivePath {m_archivePath}" +
                $" -exportOptionsPlist {exportOptionsPath}" +
                $" -exportPath {m_outputPath}";

            ToolResult result = build.Execute(command, ProjectPath.Value);
            if (result.ExitCode != 0)
            {
                throw new PostBuildStepFailedException($"{nameof(Archive)}: Failed! {result.ToString()}");
            }
#else
            throw new Exception($"{nameof(UploadToAppStore)}: unsupported platform for {m_archivePath}, {m_outputPath}, {m_pathToXCodeProject}");
#endif
        }
    }
}