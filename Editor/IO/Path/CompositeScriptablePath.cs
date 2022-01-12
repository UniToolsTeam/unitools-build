using System;
using System.IO;
using UnityEngine;

namespace UniTools.IO
{
    [CreateAssetMenu(
        fileName = nameof(CompositeScriptablePath),
        menuName = nameof(UniTools) + "/" + nameof(IO) + "/Composite Path"
    )]
    public sealed class CompositeScriptablePath : BaseScriptablePath
    {
        [SerializeField] private BaseScriptablePath m_parent = default;

        public override string ToString()
        {
            if (m_parent == null)
            {
                throw new Exception("Invalid parent path value!");
            }

            return Path.Combine(m_parent.ToString(), base.ToString());
        }
    }
}