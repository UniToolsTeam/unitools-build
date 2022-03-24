using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(CompositeScriptablePath),
        menuName = MenuPaths.IO + "Composite Path"
    )]
    public sealed class CompositeScriptablePath : BaseScriptablePath
    {
        [SerializeField] private List<PathProperty> m_paths = default;

        public override string ToString()
        {
            if (m_paths == null)
            {
                return string.Empty;
            }

            string p = string.Empty;

            foreach (PathProperty path in m_paths)
            {
                p = Path.Combine(p, path.ToString());
            }

            return p;
        }
    }
}