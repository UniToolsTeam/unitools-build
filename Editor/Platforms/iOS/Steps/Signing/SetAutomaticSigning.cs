using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    /// <summary>
    /// Enable automatic signing in the PlayerSettings.iOS
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(SetAutomaticSigning),
        menuName = MenuPaths.IOS + nameof(SetAutomaticSigning)
    )]
    public sealed class SetAutomaticSigning : ScriptableCustomBuildStep
    {
        public override async Task Execute()
        {
            PlayerSettings.iOS.iOSManualProvisioningProfileID = string.Empty;
            PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Automatic;
            PlayerSettings.iOS.appleEnableAutomaticSigning = true;

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            await Task.CompletedTask;
        }
    }
}