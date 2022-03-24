#if UNITY_IOS
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor.iOS.Xcode;
#endif

namespace UniTools.Build
{
    public sealed class ProvisioningProfile
    {
        public string Uuid { get; private set; }

        public string Name { get; private set; }
        public string TeamIdentifier { get; private set; }

        private ProvisioningProfile()
        {
        }

        public static ProvisioningProfile Load(string pathToFile)
        {
#if UNITY_IOS
            const string patternPlist = "<plist(.*)<\\/plist>";

            ProvisioningProfile profile = new ProvisioningProfile();
            string input = File.ReadAllText(pathToFile);
            Match match = Regex.Match(input, patternPlist, RegexOptions.Singleline);

            PlistDocument plistDocument = new PlistDocument();
            plistDocument.ReadFromString(match.ToString());

            //TODO Unsafe parsing! Try - catch or handle exceptions
            profile.Uuid = plistDocument.root["UUID"].AsString();
            profile.Name = plistDocument.root["Name"].AsString();
            profile.TeamIdentifier = plistDocument.root["TeamIdentifier"].AsArray().values[0].AsString();

            return profile;
#else
            throw new System.Exception($"{nameof(ProvisioningProfile)}: unsupported platform.");
#endif
        }
    }
}
