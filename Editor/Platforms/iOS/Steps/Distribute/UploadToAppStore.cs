using System.Threading.Tasks;
using UnityEngine;
#if UNITY_IOS
using System.IO;
using UnityEditor.iOS.Xcode;

#else
using System;
#endif

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(UploadToAppStore),
        menuName = MenuPaths.IOS + nameof(UploadToAppStore)
    )]
    public sealed class UploadToAppStore : DistributeIosApplicationStep
    {
        public override async Task Execute()
        {
            await Task.CompletedTask;

#if UNITY_IOS
            PlistDocument exportOptions = CreateExportOptions();

            exportOptions.root.SetString("method", "app-store");
            exportOptions.root.SetString("destination", "upload");

            File.WriteAllText(ExportOptionsPath, exportOptions.WriteToString());

            XCodeBuild build = Cli.Tool<XCodeBuild>();

            string command =
                $"-exportArchive " +
                $" -archivePath {ArchivePath}" +
                $" -exportOptionsPlist {ExportOptionsPath}" +
                $" -exportPath {OutputPath}";

            ToolResult result = build.Execute(command, ProjectPath.Value);
            if (result.ExitCode != 0)
            {
                throw new BuildStepFailedException($"{nameof(Archive)}: Failed! {result.ToString()}");
            }
#else
            throw new Exception($"{nameof(ExportIpa)}: unsupported platform!");
#endif
        }
    }
}