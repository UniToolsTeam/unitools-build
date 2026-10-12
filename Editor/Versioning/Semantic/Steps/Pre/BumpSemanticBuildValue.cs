using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(BumpSemanticBuildValue),
        menuName = MenuPaths.Versioning + nameof(BumpSemanticBuildValue)
    )]
    public sealed class BumpSemanticBuildValue : IncrementSemanticVersionStep
    {
        public override async Task Execute()
        {
            Version v = Load();
            Save(v.Major, v.Minor, checked(Math.Max(0, v.Build) + (int)Increment), v.Revision);
            await Task.CompletedTask;
        }
    }
}