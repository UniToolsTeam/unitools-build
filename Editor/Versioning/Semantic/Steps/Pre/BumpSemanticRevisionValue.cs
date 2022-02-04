using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build.Versioning
{
    [CreateAssetMenu(
        fileName = nameof(BumpSemanticRevisionValue),
        menuName = nameof(UniTools) + "/Build/Steps/" + nameof(Versioning) + "/Pre/" + nameof(BumpSemanticRevisionValue)
    )]
    public sealed class BumpSemanticRevisionValue : IncrementSemanticVersionStep
    {
        public override async Task Execute()
        {
            Version v = Load();
            Save(v.Major, v.Minor, v.Build, v.Revision + (int) Increment);
            await Task.CompletedTask;
        }
    }
}