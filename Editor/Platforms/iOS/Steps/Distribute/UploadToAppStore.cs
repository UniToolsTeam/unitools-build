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

            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(ExportOptionsPath)));
            File.WriteAllText(ExportOptionsPath, exportOptions.WriteToString());

            XCodeBuild build = Cli.Tool<XCodeBuild>();
            if (build == null || !build.IsInstalled) throw new ToolNotInstalledException();

            string command =
                $"-exportArchive " +
                $" -archivePath {CommandLine.QuoteArgument(ArchivePath)}" +
                $" -exportOptionsPlist {CommandLine.QuoteArgument(ExportOptionsPath)}" +
                $" -exportPath {CommandLine.QuoteArgument(OutputPath)}";

            ToolResult result = build.Execute(command, ProjectPath.Value);
            if (result.ExitCode != 0)
            {
                throw new BuildStepFailedException($"{nameof(Archive)}: Failed! {result.ToString()}");
            }
#else
            throw new Exception($"{nameof(UploadToAppStore)}: unsupported platform!");
#endif
        }
    }
}