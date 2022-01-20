using UnityEngine;

namespace UniTools.IO
{
    [CreateAssetMenu(
        fileName = nameof(ScriptablePath),
        menuName = nameof(UniTools) + "/" + nameof(IO) + "/Path"
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