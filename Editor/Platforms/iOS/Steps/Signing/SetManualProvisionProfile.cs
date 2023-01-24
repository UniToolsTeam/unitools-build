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
        [SerializeField] private string m_name = string.Empty;
        [SerializeField] private string m_id = string.Empty;
        [SerializeField] private ProvisioningProfileType m_type = ProvisioningProfileType.Automatic;

        public override async Task Execute()
        {
            Debug.Log($"Set ProvisioningProfile {m_name} with id {m_id} to the PlayerSettings.iOS");

            PlayerSettings.iOS.appleEnableAutomaticSigning = false;

            //TODO Get them from file
            PlayerSettings.iOS.iOSManualProvisioningProfileID = m_id;
            PlayerSettings.iOS.iOSManualProvisioningProfileType = m_type;

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            await Task.CompletedTask;
        }
    }
}