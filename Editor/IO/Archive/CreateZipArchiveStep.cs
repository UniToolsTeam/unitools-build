using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(fileName = nameof(CreateZipArchiveStep), menuName = MenuPaths.IO + "Zip Archive")]
    public sealed class CreateZipArchiveStep : BuildStep
    {
        [SerializeField, Tooltip("The folder to compress.")] private PathProperty m_directory = default;
        [SerializeField, Tooltip("Output ZIP path, relative to the source folder or absolute; keep it outside the source.")]
        private PathProperty m_archiveFileName = default;

        public override Task Execute()
        {
            string source = PathProperty.RequirePath(m_directory, "Archive source");
            string archiveName = m_archiveFileName?.ToString();
            if (string.IsNullOrWhiteSpace(archiveName) || !string.Equals(Path.GetExtension(archiveName), ".zip", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("An archive path with a .zip extension is required.");
            if (!Directory.Exists(source)) throw new DirectoryNotFoundException(source);
            string destination = Path.GetFullPath(Path.Combine(source, archiveName));
            // A ZIP inside its own input can recursively include itself or an earlier artifact.
            PathProperty.ValidateCopyPaths(source, destination);
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
            string temporary = destination + ".tmp-" + Guid.NewGuid();
            try
            {
                ZipFile.CreateFromDirectory(source, temporary, System.IO.Compression.CompressionLevel.Optimal, false);
                if (File.Exists(destination)) File.Delete(destination);
                File.Move(temporary, destination);
            }
            finally { if (File.Exists(temporary)) File.Delete(temporary); }
            return Task.CompletedTask;
        }
    }
}
