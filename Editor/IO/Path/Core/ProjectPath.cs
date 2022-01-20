using System.IO;
using UnityEngine;

namespace UniTools.IO
{
    /// <summary>
    /// The path value of the current Unity Project
    /// </summary>
    public static class ProjectPath
    {
        public static readonly string Value = Path.GetDirectoryName(Application.dataPath);
    }
}