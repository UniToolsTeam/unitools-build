using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(StringBuildParameter),
        menuName = MenuPaths.Parameters + "String"
    )]
    public sealed class StringBuildParameter : ScriptableBuildParameter<string>
    {
        protected override bool TryParseFromCommandLine(string commandLine, out string v)
        {
            if (CommandLineParser.TryToParseValue(commandLine, Name, out object obj))
            {
                v = obj.ToString();

                return true;
            }

            v = string.Empty;

            return false;
        }
    }
}