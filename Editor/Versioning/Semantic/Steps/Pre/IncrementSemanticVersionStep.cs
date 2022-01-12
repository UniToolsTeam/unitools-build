using UnityEngine;

namespace UniTools.Build.Versioning.Semantic
{
    public abstract class IncrementSemanticVersionStep : ChangeSemanticVersionStep
    {
        [SerializeField] protected uint Increment = 1;
    }
}