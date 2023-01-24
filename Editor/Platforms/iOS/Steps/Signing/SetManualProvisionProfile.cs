using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(SetManualProvisionProfile),
        menuName = MenuPaths.IOS + nameof(SetManualProvisionProfile)
    )]
    public sealed class SetManualProvisionProfile : ScriptableCustomBuildStep
    {
        public override async Task Execute()
        {
            PlayerSettings.iOS.appleEnableAutomaticSigning = false;

            //TODO Get them from file
            PlayerSettings.iOS.iOSManualProvisioningProfileID = "Hello world";
            PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Development;

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            await Task.CompletedTask;
        }
    }
}