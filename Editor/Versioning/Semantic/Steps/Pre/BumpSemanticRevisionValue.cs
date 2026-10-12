using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(BumpSemanticRevisionValue),
        menuName = MenuPaths.Versioning + nameof(BumpSemanticRevisionValue)
    )]
    public sealed class BumpSemanticRevisionValue : IncrementSemanticVersionStep
    {
        public override async Task Execute()
        {
            Version v = Load();
            Save(v.Major, v.Minor, Math.Max(0, v.Build), checked(Math.Max(0, v.Revision) + (int)Increment));
            await Task.CompletedTask;
        }
    }
}