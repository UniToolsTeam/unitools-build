using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build.Versioning
{
    [CreateAssetMenu(
        fileName = nameof(BumpSemanticMinorValue),
        menuName = MenuPaths.Versioning + nameof(BumpSemanticMinorValue)
    )]
    public sealed class BumpSemanticMinorValue : IncrementSemanticVersionStep
    {
        public override async Task Execute()
        {
            Version v = Load();
            Save(v.Major, v.Minor + (int)Increment, v.Build, v.Revision);
            await Task.CompletedTask;
        }
    }
}