using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build.Android
{
    [CreateAssetMenu(
        fileName = nameof(SetDebugKeystore),
        menuName = nameof(UniTools) + "/Build/Steps/" + nameof(Android) + "/Pre/" + nameof(SetDebugKeystore)
    )]
    public sealed class SetDebugKeystore : ScriptableCustomBuildStep
    {
        public override async Task Execute()
        {
            PlayerSettings.Android.useCustomKeystore = false;
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            await Task.CompletedTask;
        }
    }
}