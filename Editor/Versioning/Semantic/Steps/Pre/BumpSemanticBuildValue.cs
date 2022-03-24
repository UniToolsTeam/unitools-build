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
            Save(v.Major, v.Minor, v.Build + (int)Increment, v.Revision);
            await Task.CompletedTask;
        }
    }
}