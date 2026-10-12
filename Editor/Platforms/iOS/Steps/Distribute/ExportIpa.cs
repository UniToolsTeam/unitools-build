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
        fileName = nameof(ExportIpa),
        menuName = MenuPaths.IOS + nameof(ExportIpa)
    )]
    public sealed class ExportIpa : DistributeIosApplicationStep
    {
#pragma warning disable CS0414
        // Keep the saved Inspector field on non-iOS targets; only iOS code consumes it.
        [SerializeField, Tooltip("The method must correspond to the provision profile")] private ExportMethods m_method = ExportMethods.AdHoc;
#pragma warning restore CS0414

        public override async Task Execute()
        {
            await Task.CompletedTask;
#if UNITY_IOS
            PlistDocument exportOptions = CreateExportOptions();

            if (m_method == ExportMethods.AdHoc)
            {
                exportOptions.root.SetString("method", "ad-hoc");
            }
            else if (m_method == ExportMethods.AppStore)
            {
                exportOptions.root.SetString("method", "app-store");
            }
            else
            {
                throw new BuildStepFailedException($"{nameof(Archive)}: Failed due to unsupported method {m_method}");
            }

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
            throw new Exception($"{nameof(ExportIpa)}: unsupported platform!");
#endif
        }
    }
}
