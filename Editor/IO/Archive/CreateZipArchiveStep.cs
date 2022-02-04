using System;
using System.IO;
using System.Threading.Tasks;
using UniTools.Build;
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
        [SerializeField, Tooltip("The name of the created archive (include extensions. Ex: my.zip).")] private PathProperty m_archiveFileName = default;

        public override async Task Execute()
        {
            if (string.IsNullOrWhiteSpace(m_archiveFileName.ToString()))
            {
                throw new Exception($"{nameof(CreateZipArchiveStep)}: file name is null or empty!");
            }

            string ext = Path.GetExtension(m_archiveFileName.ToString());
            if (!ext.Equals(".zip"))
            {
                throw new Exception($"{nameof(CreateZipArchiveStep)}: invalid file name {m_archiveFileName}!");
            }

            if (!Directory.Exists(m_directory.ToString()))
            {
                throw new Exception($"{nameof(CreateZipArchiveStep)}: failed due to directory {m_directory} doesn't exist!");
            }

            Zip zip = Cli.Tool<Zip>();
            if (zip == null || !zip.IsInstalled)
            {
                throw new ToolNotInstalledException();
            }

            string command = $"-r {m_archiveFileName} *";
            ToolResult result = zip.Execute(command, m_directory.ToString());
            Debug.Log(result);

            await Task.CompletedTask;
        }
    }
}