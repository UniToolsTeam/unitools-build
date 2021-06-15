using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class ScriptablePreBuildStep : ScriptableObject
    {
        public abstract Task Execute();
    }
}