#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
namespace UniTools.Build.iOS
{
    public interface IPlistElement
    {
        void AddTo(
#if UNITY_IOS
            PlistElementDict plistElementDict
#endif
        );
    }
}
