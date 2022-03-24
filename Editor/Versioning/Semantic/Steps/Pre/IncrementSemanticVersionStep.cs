using UnityEngine;

namespace UniTools.Build
{
    public abstract class IncrementSemanticVersionStep : ChangeSemanticVersionStep
    {
        [SerializeField] protected uint Increment = 1;
    }
}