using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
#if UNITY_WEBGL
using UnityEditor.WebGL;
#endif

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(BuildWebGL),
        menuName = MenuPaths.WebGL + nameof(BuildWebGL)
    )]
    public sealed class BuildWebGL : UnityBuildStepWithOptions
    {
#if UNITY_WEBGL
        [SerializeField] private CodeOptimization m_codeOptimization = CodeOptimization.Size;
#endif

#if UNITY_2021_2_OR_NEWER
        [SerializeField] private Il2CppCodeGeneration m_IL2CPPCodeGeneration = Il2CppCodeGeneration.OptimizeSize;
#endif

        [SerializeField] private WebGLExceptionSupport m_exceptionSupport = WebGLExceptionSupport.None;
        [SerializeField] private WebGLCompressionFormat m_compressionFormat = default;
        [SerializeField] private bool m_nameFilesAsHashes = true;

        [SerializeField] private bool m_dataCaching = false;

#if UNITY_2021_2_OR_NEWER
        [SerializeField] private WebGLDebugSymbolMode m_debugSymbolMode = WebGLDebugSymbolMode.Off;
#endif
#if UNITY_2020_1_OR_NEWER
        [SerializeField] private bool m_decompressionFallback = false;
#endif
        public override BuildTarget Target => BuildTarget.WebGL;

        public override async Task Execute()
        {
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            PlayerSettings.WebGL.exceptionSupport = m_exceptionSupport;
            PlayerSettings.WebGL.compressionFormat = m_compressionFormat;
            PlayerSettings.WebGL.nameFilesAsHashes = m_nameFilesAsHashes;
            PlayerSettings.WebGL.dataCaching = m_dataCaching;
#if UNITY_2021_2_OR_NEWER
            PlayerSettings.WebGL.debugSymbolMode = m_debugSymbolMode;
#endif

#if UNITY_2020_1_OR_NEWER
            PlayerSettings.WebGL.decompressionFallback = m_decompressionFallback;
#endif

#if UNITY_WEBGL
            EditorUserBuildSettings.SetPlatformSettings(UnityEditor.BuildPipeline.GetBuildTargetName(BuildTarget.WebGL), "CodeOptimization", m_codeOptimization.ToString());
#else
            EditorUserBuildSettings.SetPlatformSettings(UnityEditor.BuildPipeline.GetBuildTargetName(BuildTarget.WebGL), "CodeOptimization", "Size");
#endif

#if UNITY_2022_1_OR_NEWER
            PlayerSettings.SetIl2CppCodeGeneration(NamedBuildTarget.WebGL, m_IL2CPPCodeGeneration);
#elif UNITY_2021_2_OR_NEWER
            EditorUserBuildSettings.il2CppCodeGeneration = m_IL2CPPCodeGeneration;
#endif

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            BuildReport report = UnityEditor.BuildPipeline.BuildPlayer(Options);

            await Task.CompletedTask;

            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Failed)
            {
                throw new Exception($"{nameof(BuildPipeline)}: {name} Build failed!");
            }
        }
    }
}