using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class ScriptableBuildPipeline : ScriptableObject
    {
        public abstract Task Run();
    }
}