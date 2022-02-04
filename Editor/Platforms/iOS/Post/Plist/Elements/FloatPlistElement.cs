using System;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
namespace UniTools.Build.iOS
{
    [Serializable]
    public sealed class FloatPlistElement : SerializablePlistElement<float>
    {
#if UNITY_IOS
        public override void AddTo(PlistElementDict plistElementDict)
        {
            plistElementDict.SetReal(Key, Value);
        }
#endif
    }
}