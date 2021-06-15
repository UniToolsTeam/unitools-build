using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class ScriptablePostBuildStep : ScriptableObject
    {
        public abstract Task Execute(string pathToBuiltProject);
    }
}