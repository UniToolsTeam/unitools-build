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
#pragma warning disable
        //Pragma used to avoid miss values on another platform. Can't not be closed #if UNITY_IOS
        [SerializeField, Tooltip("The method must correspond to the provision profile")] private ExportMethods m_method = ExportMethods.AdHoc;
#pragma warning restore

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