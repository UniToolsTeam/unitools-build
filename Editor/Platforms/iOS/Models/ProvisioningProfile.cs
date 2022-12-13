#if UNITY_IOS
using System.Xml;
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

            string xml = match.ToString();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            foreach (XmlNode parentNode in xmlDoc.DocumentElement)
            {
                for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                {
                    if (parentNode.ChildNodes[i].InnerText == "UUID")
                    {
                        profile.Uuid = parentNode.ChildNodes[i + 1].InnerText;
                    }

                    if (parentNode.ChildNodes[i].InnerText == "Name")
                    {
                        profile.Name = parentNode.ChildNodes[i + 1].InnerText;
                    }

                    if (parentNode.ChildNodes[i].InnerText == "TeamIdentifier")
                    {
                        profile.TeamIdentifier = parentNode.ChildNodes[i + 1].InnerText;
                    }
                }
            }

            return profile;
#else
            throw new System.Exception($"{nameof(ProvisioningProfile)}: unsupported platform.");
#endif
        }
    }
}
