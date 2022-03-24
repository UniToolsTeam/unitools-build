using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace UniTools.Build.Android
{
    [CreateAssetMenu(
        fileName = nameof(SetAlias),
        menuName = MenuPaths.Android + nameof(SetAlias)
    )]
    public sealed class SetAlias : ScriptableCustomBuildStep
    {
        [SerializeField] private string m_alias = default;
        [SerializeField] private string m_password = default;

        [ContextMenu(nameof(Execute))]
        public override async Task Execute()
        {
            if (!PlayerSettings.Android.useCustomKeystore)
            {
                throw new BuildFailedException($"{nameof(SetAlias)}: failed due to using of the custom keystore is not selected!");
            }

            PlayerSettings.Android.keyaliasName = m_alias;
            PlayerSettings.Android.keyaliasPass = m_password;
            PlayerSettings.keyaliasPass = m_password;
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            await Task.CompletedTask;
        }
    }
}