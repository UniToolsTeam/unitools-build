using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build.Versioning
{
    [CreateAssetMenu(
        fileName = nameof(BumpSemanticBuildValue),
        menuName = nameof(UniTools) + "/Build/Steps/" + nameof(Versioning) + "/Pre/" + nameof(BumpSemanticBuildValue)
    )]
    public sealed class BumpSemanticBuildValue : IncrementSemanticVersionStep
    {
        public override async Task Execute()
        {
            Version v = Load();
            Save(v.Major, v.Minor, v.Build + (int) Increment, v.Revision);
            await Task.CompletedTask;
        }
    }
}