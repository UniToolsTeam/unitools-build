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
            Save(v.Major, v.Minor, v.Build, v.Revision + (int)Increment);
            await Task.CompletedTask;
        }
    }
}