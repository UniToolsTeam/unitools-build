using System;
using UnityEditor;

namespace UniTools.Build
{
    public abstract class ChangeSemanticVersionStep : ScriptableCustomBuildStep
    {
        protected Version Load()
        {
            if (Version.TryParse(PlayerSettings.bundleVersion, out Version version))
            {
                return version;
            }

            throw new VersionIsNotSemanticException($"Invalid version format in {nameof(PlayerSettings)}! Current is version {PlayerSettings.bundleVersion} is not semantic!");
        }

        protected void Save(int major, int minor, int build, int revision)
        {
            if (build == -1)
            {
                PlayerSettings.bundleVersion =
                    $"{major.ToString()}.{minor.ToString()}";
            }
            else if (revision == -1)
            {
                PlayerSettings.bundleVersion =
                    $"{major.ToString()}.{minor.ToString()}.{build.ToString()}";
            }
            else
            {
                PlayerSettings.bundleVersion =
                    $"{major.ToString()}.{minor.ToString()}.{build.ToString()}.{revision.ToString()}";
            }

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
    }
}