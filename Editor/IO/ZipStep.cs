using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UniTools.Build;
using UniTools.CLI;
using UnityEngine;

namespace UniTools.IO
{
    [CreateAssetMenu(
        fileName = nameof(ZipStep),
        menuName = nameof(UniTools) + "/" + nameof(IO) + "/Zip"
    )]
    public sealed class ZipStep : ScriptablePostBuildStep
    {
        [SerializeField, Tooltip("The location of the folder that needs to be compressed.")] private PathProperty m_directory = default;
        [SerializeField, Tooltip("The name of the created archive (include extensions. Ex: my.zip).")] private string m_archiveFileName = default;

        public override async Task Execute(string pathToBuiltProject)
        {
            
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