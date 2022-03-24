#if UNITY_IOS
using System.IO;
using UnityEditor.iOS.Xcode;
#else
using System;
#endif
using System.Threading.Tasks;
using UniTools.Build;
using UnityEngine;

namespace UniTools.Build.iOS
{
    [CreateAssetMenu(
        fileName = nameof(ExportIpa),
        menuName = MenuPaths.IOS + nameof(ExportIpa)
    )]
    public sealed class ExportIpa : DistributeIosApplicationStep
    {
        [SerializeField] private PathProperty m_pathToXCodeProject = default;
        [SerializeField] private PathProperty m_archivePath = new PathProperty("Unity-iPhone.xcarchive");
        [SerializeField] private PathProperty m_outputPath = default;
        [SerializeField, Tooltip("The method must correspond to the provision profile")] private ExportMethods m_method = ExportMethods.AdHoc;

        public override async Task Execute()
        {
            await Task.CompletedTask;
#if UNITY_IOS
            PlistDocument exportOptions = CreateExportOptions();
            string exportOptionsPath = ExportOptionsPath(m_pathToXCodeProject.ToString());

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
                throw new PostBuildStepFailedException($"{nameof(Archive)}: Failed due to unsupported method {m_method}");
            }

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
            throw new Exception($"{nameof(ExportIpa)}: unsupported platform for {m_archivePath}, {m_outputPath}, {m_method}, {m_pathToXCodeProject}");
#endif
        }
    }
}