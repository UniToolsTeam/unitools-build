using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(ScriptablePath),
        menuName = MenuPaths.IO + "Path"
    )]
    public sealed class ScriptablePath : BaseScriptablePath
    {
        [SerializeField] private string m_value = default;

        public string Value
        {
            set => m_value = value;
        }

        public override string ToString()
        {
            return m_value;
        }
    }
}