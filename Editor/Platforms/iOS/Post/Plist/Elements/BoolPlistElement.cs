using System;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
namespace UniTools.Build.iOS
{
    [Serializable]
    public sealed class BoolPlistElement : SerializablePlistElement<bool>
    {
#if UNITY_IOS
        public override void AddTo(PlistElementDict plistElementDict)
        {
            plistElementDict.SetBoolean(Key, Value);
        }
#endif
    }
}
