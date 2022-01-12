using System;
using UnityEngine;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

namespace UniTools.Build.iOS
{
    [Serializable]
    public abstract class SerializablePlistElement<TValue>
        : IPlistElement
    {
        [SerializeField] protected string Key;
        [SerializeField] protected TValue Value;

#if UNITY_IOS
        public abstract void AddTo(PlistElementDict plistElementDict);
#else
        public void AddTo()
        {
            throw new Exception($"{nameof(SerializablePlistElement<TValue>)}:Unsupported platform for {Key} and {Value}");
        }

#endif
    }
}