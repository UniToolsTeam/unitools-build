using UnityEngine;

namespace UniTools.Build.Versioning
{
    public abstract class IncrementSemanticVersionStep : ChangeSemanticVersionStep
    {
        [SerializeField] protected uint Increment = 1;
    }
}