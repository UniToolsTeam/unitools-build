#if UNITY_IOS
using System.IO;
using UnityEditor.iOS.Xcode;
#else
using System;
#endif
using System.Threading.Tasks;
using UniTools.Build;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(UploadToAppStore),
        menuName = MenuPaths.IOS + nameof(UploadToAppStore)
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
                throw new BuildStepFailedException($"{nameof(Archive)}: Failed! {result.ToString()}");
            }
#else
            throw new Exception($"{nameof(UploadToAppStore)}: unsupported platform for {m_archivePath}, {m_outputPath}, {m_pathToXCodeProject}");
#endif
        }
    }
}