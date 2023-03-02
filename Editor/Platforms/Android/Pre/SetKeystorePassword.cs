using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(SetKeystorePassword),
        menuName = MenuPaths.Android + nameof(SetKeystorePassword)
    )]
    public sealed class SetKeystorePassword : BuildStep
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