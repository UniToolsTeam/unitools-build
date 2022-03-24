using System;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
namespace UniTools.Build
{
    [Serializable]
    public sealed class IntPlistElement : SerializablePlistElement<int>
    {
#if UNITY_IOS
        public override void AddTo(PlistElementDict plistElementDict)
        {
            plistElementDict.SetInteger(Key, Value);
        }
#endif
    }
}