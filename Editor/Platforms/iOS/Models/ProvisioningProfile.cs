using System;
#if UNITY_IOS
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
#endif

namespace UniTools.Build
{
    public sealed class ProvisioningProfile
    {
        public static readonly ProvisioningProfile Empty = new ProvisioningProfile
        { Uuid = string.Empty, Name = string.Empty, TeamIdentifier = string.Empty };
        public string Uuid { get; private set; }
        public string Name { get; private set; }
        public string TeamIdentifier { get; private set; }
        private ProvisioningProfile() { }

        public static ProvisioningProfile Load(string pathToFile)
        {
            if (string.IsNullOrWhiteSpace(pathToFile)) return Empty;
#if UNITY_IOS
            string input = File.ReadAllText(pathToFile);
            Match match = Regex.Match(input, "<plist[\\s\\S]*?</plist>");
            if (!match.Success) throw new FormatException("No plist was found in the provisioning profile.");
            var document = new XmlDocument { XmlResolver = null };
            document.LoadXml(match.Value);
            XmlNode dictionary = document.SelectSingleNode("/plist/dict");
            if (dictionary == null) throw new FormatException("The provisioning profile has no dictionary.");
            var profile = new ProvisioningProfile();
            foreach (XmlNode key in dictionary.ChildNodes)
            {
                if (key.Name != "key") continue;
                XmlNode value = key.NextSibling;
                if (value == null) throw new FormatException("Missing provisioning profile value for " + key.InnerText);
                if (key.InnerText == "UUID") profile.Uuid = value.InnerText;
                if (key.InnerText == "Name") profile.Name = value.InnerText;
                if (key.InnerText == "TeamIdentifier") profile.TeamIdentifier = value.SelectSingleNode("string")?.InnerText;
            }
            if (string.IsNullOrWhiteSpace(profile.Uuid) || string.IsNullOrWhiteSpace(profile.Name)
                || string.IsNullOrWhiteSpace(profile.TeamIdentifier))
                throw new FormatException("The provisioning profile is missing UUID, Name or TeamIdentifier.");
            return profile;
#else
            throw new PlatformNotSupportedException("Provisioning profiles require the active iOS target and iOS module.");
#endif
        }
    }
}
