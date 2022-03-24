#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class DistributeIosApplicationStep : IosPostBuildStep
    {
        [SerializeField] private string m_bundleIdentifier = default;
        [SerializeField] private bool m_uploadBitcode = false;
        [SerializeField] private bool m_uploadSymbols = false;

        protected string ExportOptionsPath(string root)
        {
            const string fileName = "ExportOptions.plist";

            return Path.Combine(root, fileName);
#if !UNITY_IOS
            throw new System.Exception($"{nameof(DistributeIosApplicationStep)}: unsupported platform for {m_bundleIdentifier}, {m_uploadBitcode}, {m_uploadSymbols}");
#endif
        }

#if UNITY_IOS
        protected PlistDocument CreateExportOptions()
        {
            PlistDocument plist = new PlistDocument();
            PlistElementDict rootDict = plist.root;

            rootDict.SetBoolean("generateAppStoreInformation", false);

            PlistElementDict provisioningProfiles = rootDict.CreateDict("provisioningProfiles");
            provisioningProfiles.SetString(m_bundleIdentifier, ProvisioningProfileName);
            rootDict.SetString("signingCertificate", "Apple Distribution");
            rootDict.SetString("signingStyle", "manual");
            rootDict.SetString("teamID", TeamId);
            rootDict.SetBoolean("stripSwiftSymbols", true);
            rootDict.SetBoolean("uploadBitcode", m_uploadBitcode);
            rootDict.SetBoolean("uploadSymbols", m_uploadSymbols);

            return plist;
        }
#endif
    }
}
