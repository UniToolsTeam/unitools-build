using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(StringBuildParameter),
        menuName = MenuPaths.Parameters + "Int"
    )]
    public sealed class IntBuildParameter : ScriptableBuildParameter<int>
    {
        protected override bool TryParseFromCommandLine(string commandLine, out int v)
        {
            if (CommandLineParser.TryToParseValue(commandLine, CliKey, out object obj))
            {
                if (int.TryParse(obj.ToString(), out v))
                {
                    return true;
                }
            }

            v = int.MinValue;

            return false;
        }
    }
}