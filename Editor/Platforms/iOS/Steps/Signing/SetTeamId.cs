using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    /// <summary>
    /// Set iOS team id in the PlayerSettings.iOS
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(SetTeamId),
        menuName = MenuPaths.IOS + nameof(SetTeamId)
    )]
    public sealed class SetTeamId : ScriptableCustomBuildStep
    {
        [SerializeField] private string m_teamId = default;

        public override async Task Execute()
        {
            PlayerSettings.iOS.appleDeveloperTeamID = m_teamId;

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            await Task.CompletedTask;
        }
    }
}