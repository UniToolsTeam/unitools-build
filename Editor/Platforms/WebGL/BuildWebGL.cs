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
    public sealed class BuildWebGL : ScriptableBuildStepWithOptions
    {
#if UNITY_WEBGL
        [SerializeField] private CodeOptimization m_codeOptimization = CodeOptimization.Size;
#endif
        [SerializeField] private Il2CppCodeGeneration m_IL2CPPCodeGeneration = Il2CppCodeGeneration.OptimizeSize;
        [SerializeField] private WebGLExceptionSupport m_exceptionSupport = WebGLExceptionSupport.None;
        [SerializeField] private WebGLCompressionFormat m_compressionFormat = default;
        [SerializeField] private bool m_nameFilesAsHashes = true;
        [SerializeField] private bool m_dataCaching = false;
        [SerializeField] private WebGLDebugSymbolMode m_debugSymbolMode = WebGLDebugSymbolMode.Off;
        [SerializeField] private bool m_decompressionFallback = false;

        public override BuildTarget Target => BuildTarget.WebGL;

        public override async Task<BuildReport> Execute()
        {
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            PlayerSettings.WebGL.exceptionSupport = m_exceptionSupport;
            PlayerSettings.WebGL.compressionFormat = m_compressionFormat;
            PlayerSettings.WebGL.nameFilesAsHashes = m_nameFilesAsHashes;
            PlayerSettings.WebGL.dataCaching = m_dataCaching;
            PlayerSettings.WebGL.debugSymbolMode = m_debugSymbolMode;
            PlayerSettings.WebGL.decompressionFallback = m_decompressionFallback;
#if UNITY_WEBGL
            EditorUserBuildSettings.SetPlatformSettings(UnityEditor.BuildPipeline.GetBuildTargetName(BuildTarget.WebGL), "CodeOptimization", m_codeOptimization.ToString());
#else
            EditorUserBuildSettings.SetPlatformSettings(UnityEditor.BuildPipeline.GetBuildTargetName(BuildTarget.WebGL), "CodeOptimization", "Size");
#endif
            EditorUserBuildSettings.il2CppCodeGeneration = m_IL2CPPCodeGeneration;

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            BuildReport report = UnityEditor.BuildPipeline.BuildPlayer(Options);

            await Task.CompletedTask;

            return report;
        }
    }

#endif
        }