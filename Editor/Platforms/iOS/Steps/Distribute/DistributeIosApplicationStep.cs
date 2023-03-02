using System.IO;
using UnityEngine;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

namespace UniTools.Build
{
    public abstract class DistributeIosApplicationStep : BuildStep
    {
        [SerializeField, Tooltip("Can be found at Apple Developer Console")] private string m_teamId = string.Empty;
        [SerializeField] private string m_provisioningProfileName = default;
        [SerializeField] private string m_bundleIdentifier = default;
        [SerializeField] private bool m_uploadBitcode = false;
        [SerializeField] private bool m_uploadSymbols = false;
        [SerializeField] private PathProperty m_pathToXCodeProject = new PathProperty("Builds/xCode");
        [SerializeField] private PathProperty m_archivePath = new PathProperty("Unity-iPhone.xcarchive");
        [SerializeField, Tooltip("Must be a folder")] private PathProperty m_outputPath = new PathProperty("Builds");

        protected string ExportOptionsPath
        {
            get
            {
                const string fileName = "ExportOptions.plist";

                return Path.Combine(m_pathToXCodeProject.ToString(), fileName);

#if !UNITY_IOS
                throw new System.Exception($"{nameof(DistributeIosApplicationStep)}: unsupported platform for {m_bundleIdentifier}, {m_uploadBitcode}, {m_uploadSymbols}");
#endif
            }
        }

        protected string ArchivePath => m_archivePath.ToString();
        protected string OutputPath => m_outputPath.ToString();

#if UNITY_IOS
        protected PlistDocument CreateExportOptions()
        {
            PlistDocument plist = new PlistDocument();
            PlistElementDict rootDict = plist.root;

            rootDict.SetBoolean("generateAppStoreInformation", false);

            PlistElementDict provisioningProfiles = rootDict.CreateDict("provisioningProfiles");
            provisioningProfiles.SetString(m_bundleIdentifier, m_provisioningProfileName);
            rootDict.SetString("signingCertificate", "Apple Distribution");
            rootDict.SetString("signingStyle", "manual");
            rootDict.SetString("teamID", m_teamId);
            rootDict.SetBoolean("stripSwiftSymbols", true);
            rootDict.SetBoolean("uploadBitcode", m_uploadBitcode);
            rootDict.SetBoolean("uploadSymbols", m_uploadSymbols);

            return plist;
        }
#endif
    }
}