using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(SetDebugKeystore),
        menuName = MenuPaths.Android + nameof(SetDebugKeystore)
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