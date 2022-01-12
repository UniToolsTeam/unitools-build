using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build.Versioning.Semantic
{
    [CreateAssetMenu(
        fileName = nameof(BumpSemanticMajorValue),
        menuName = nameof(UniTools) + "/Build/Steps/" + nameof(Versioning) + "/Pre/" + nameof(BumpSemanticMajorValue)
    )]
    public sealed class BumpSemanticMajorValue : IncrementSemanticVersionStep
    {
        public override async Task Execute()
        {
            Version v = Load();
            Save(v.Major + (int) Increment, v.Minor, v.Build, v.Revision);
            await Task.CompletedTask;
        }
    }
}