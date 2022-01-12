using System;
using System.IO;
using System.Threading.Tasks;
using UniTools.CLI;
using UniTools.IO;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;

namespace UniTools.Build.iOS
{
    [CreateAssetMenu(
        fileName = nameof(UploadToAppStore),
        menuName = nameof(UniTools) + "/Build/Steps/" + nameof(iOS) + "/Post/" + nameof(UploadToAppStore)
    )]
    public sealed class UploadToAppStore : DistributeIosApplicationStep
    {
        [SerializeField] private PathProperty m_archivePath = new PathProperty("Unity-iPhone.xcarchive");
        [SerializeField] private PathProperty m_outputPath = default;

        public override async Task Execute(string pathToBuiltProject)
        {
            await Task.CompletedTask;

#if UNITY_IOS
            PlistDocument exportOptions = CreateExportOptions();
            string exportOptionsPath = ExportOptionsPath(pathToBuiltProject);

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
            throw new Exception($"{nameof(UploadToAppStore)}: unsupported platform for {m_archivePath}, {m_outputPath}");
#endif
        }
    }
}
