using System;
using System.IO;
using System.Threading.Tasks;
using UniTools.Build;
using UniTools.CLI;
using UnityEngine;

namespace UniTools.IO
{
    [CreateAssetMenu(
        fileName = nameof(CreateZipArchiveStep),
        menuName = nameof(UniTools) + "/" + nameof(IO) + "/" + nameof(CreateZipArchiveStep)
    )]
    public sealed class CreateZipArchiveStep : ScriptableCustomBuildStep
    {
        [SerializeField, Tooltip("The location of the folder that needs to be compressed.")] private PathProperty m_directory = default;
        [SerializeField, Tooltip("The name of the created archive (include extensions. Ex: my.zip).")] private string m_archiveFileName = default;

        public override async Task Execute()
        {
            if (string.IsNullOrWhiteSpace(m_archiveFileName))
            {
                throw new Exception($"{nameof(CreateZipArchiveStep)}: file name is null or empty!");
            }

            var ext = Path.GetExtension(m_archiveFileName);
            Debug.LogError(ext);
            if (!ext.Equals(".zip"))
            {
                throw new Exception($"{nameof(CreateZipArchiveStep)}: invalid file name {m_archiveFileName}!");
            }

            // var dir = Path.Combine(Environment.CurrentDirectory, m_directory.ToString());
            //
            // if (!Directory.Exists(dir))
            // {
            //     throw new Exception($"{nameof(CreateZipArchiveStep)}: failed due to directory {dir} doesn't exist!");
            // }
            //
            //

            if (!Directory.Exists(m_directory.ToString()))
            {
                throw new Exception($"{nameof(CreateZipArchiveStep)}: failed due to directory {m_directory} doesn't exist!");
            }

            // Debug.Log(Assembly.GetExecutingAssembly().De);
            // Debug.Log(Environment.CurrentDirectory);
            // // Debug.Log(Application.dataPath);
            //
            // Debug.LogError(m_directory);
            //
            // var p = Path.Combine(Environment.CurrentDirectory, m_directory.ToString());
            // Debug.LogError(p);
            //
            // FileAttributes attr = File.GetAttributes(p.ToString());
            //
            //
            // Debug.LogError(attr);
            //
            // if (attr.HasFlag(FileAttributes.Directory))
            // {
            //     Debug.Log("The target is DIRECTORY");
            //     // MessageBox.Show("Its a directory");
            // }
            // else
            // {
            //     Debug.Log("The target is a FILE");
            //
            //     // MessageBox.Show("Its a file");
            // }
            //
            //
            //
            //
            //  // Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "ARTIFACTS"));
            //
            // return;
            //

            // tar -czvf archive.zip *   
            Tar tar = Cli.Tool<Tar>();
            if (tar == null || !tar.IsInstalled)
            {
                throw new ToolNotInstalledException();
            }

            string command = $"-czf {m_archiveFileName} *";
            // string command = $"-c -f {m_archiveFileName} {m_directory}";
            // string command = $"-a -c -f {m_archivePath} {m_originalPath}";

            var result = tar.Execute(command, m_directory.ToString());

            Debug.Log(result);

            await Task.CompletedTask;
        }
    }
}