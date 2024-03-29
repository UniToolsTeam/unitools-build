#if UNITY_IOS
using System.Collections.Generic;
using System.IO;
using UnityEditor.iOS.Xcode;
#else
using System;
#endif
using System.Threading.Tasks;
using UniTools.Build;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(ModifyInfoPlist),
        menuName = MenuPaths.IOS + nameof(ModifyInfoPlist)
    )]
    public sealed class ModifyInfoPlist : BuildStep
    {
        [SerializeField] private PathProperty m_pathToXCodeProject = default;
        [SerializeField] private BoolPlistElement[] m_bool = default;
        [SerializeField] private FloatPlistElement[] m_float = default;
        [SerializeField] private IntPlistElement[] m_int = default;
        [SerializeField] private StringPlistElement[] m_string = default;

        public override async Task Execute()
        {
            await Task.CompletedTask;

#if UNITY_IOS
            List<IPlistElement> elements = new List<IPlistElement>();
            if (m_bool != null)
            {
                elements.AddRange(m_bool);
            }

            if (m_float != null)
            {
                elements.AddRange(m_float);
            }

            if (m_int != null)
            {
                elements.AddRange(m_int);
            }

            if (m_string != null)
            {
                elements.AddRange(m_string);
            }

            string plistPath = Path.Combine(m_pathToXCodeProject.ToString(), "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            foreach (IPlistElement element in elements)
            {
                element.AddTo(plist.root);
            }

            File.WriteAllText(plistPath, plist.WriteToString());
#else
            throw new Exception($"{nameof(ModifyInfoPlist)}: unsupported platform for {m_bool}, {m_float}, {m_int}, {m_string}, {m_pathToXCodeProject}");
#endif
        }
    }
}