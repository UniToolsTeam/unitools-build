using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build.Android
{
    [CreateAssetMenu(
        fileName = nameof(SetKeystorePassword),
        menuName = nameof(UniTools) + "/Build/Steps/" + nameof(Android) + "/Pre/" + nameof(SetKeystorePassword)
    )]
    public sealed class SetKeystorePassword : ScriptableCustomBuildStep
    {
        [SerializeField] private string m_password = default;

        [ContextMenu(nameof(Execute))]
        public override async Task Execute()
        {
            PlayerSettings.Android.useCustomKeystore = true;

            PlayerSettings.keystorePass = m_password;

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            await Task.CompletedTask;
        }
    }
}