using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public static class BatchModeBuilder
    {
        private const string m_guidKey = "UniTools.Build.Batch.Guid";
        private const string m_indexKey = "UniTools.Build.Batch.NextStep";
        private static Task m_stepTask;
        private static int m_waitUpdates;

        // executeMethod returns immediately; the update runner owns completion and the exit code.
        public static void Execute()
        {
            if (!Application.isBatchMode)
                throw new InvalidOperationException("BatchModeBuilder requires -batchmode.");
            try
            {
                string[] arguments = Environment.GetCommandLineArgs();
                if (arguments.Contains("-quit"))
                    throw new ArgumentException("Do not pass -quit: BatchModeBuilder exits after all asynchronous steps finish.");
                if (!CommandLineParser.TryToParseValue(arguments, "--pipeline", out object value)
                    || string.IsNullOrWhiteSpace(value.ToString()))
                    throw new ArgumentException("Specify --pipeline followed by a pipeline asset name or path.");
                BuildPipeline pipeline = Find(value.ToString());
                if (pipeline.Count == 0) throw new BuildStepFailedException("The pipeline has no steps.");
                SessionState.SetString(m_guidKey, AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(pipeline)));
                SessionState.SetInt(m_indexKey, 0);
                Resume();
            }
            catch (Exception exception) { Finish(exception); }
        }

        [InitializeOnLoadMethod]
        private static void Resume()
        {
            if (!Application.isBatchMode || string.IsNullOrEmpty(SessionState.GetString(m_guidKey, ""))) return;
            m_stepTask = null;
            m_waitUpdates = 2;
            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
        }

        private static BuildPipeline Find(string nameOrPath)
        {
            BuildPipeline byPath = AssetDatabase.LoadAssetAtPath<BuildPipeline>(nameOrPath);
            if (byPath != null) return byPath;
            BuildPipeline found = null;
            foreach (string guid in AssetDatabase.FindAssets("t:BuildPipeline"))
            {
                BuildPipeline pipeline = AssetDatabase.LoadAssetAtPath<BuildPipeline>(AssetDatabase.GUIDToAssetPath(guid));
                if (pipeline == null || !string.Equals(pipeline.name, nameOrPath, StringComparison.Ordinal)) continue;
                if (found != null) throw new ArgumentException("Duplicate pipeline names; use the asset path instead.");
                found = pipeline;
            }
            return found != null ? found : throw new ArgumentException("Pipeline not found: " + nameOrPath);
        }

        private static void Tick()
        {
            try
            {
                if (m_stepTask != null)
                {
                    if (!m_stepTask.IsCompleted) return;
                    CompleteStep();
                }
                // Persist progress before a define-changing step triggers compilation/domain reload.
                if (EditorApplication.isCompiling || EditorApplication.isUpdating) return;
                if (m_waitUpdates-- > 0) return;
                if (EditorUtility.scriptCompilationFailed)
                    throw new BuildStepFailedException("Compilation failed during the pipeline.");
                string path = AssetDatabase.GUIDToAssetPath(SessionState.GetString(m_guidKey, ""));
                BuildPipeline pipeline = AssetDatabase.LoadAssetAtPath<BuildPipeline>(path);
                if (pipeline == null) throw new BuildStepFailedException("The running pipeline asset was removed.");
                int index = SessionState.GetInt(m_indexKey, 0);
                if (index >= pipeline.Count) { Finish(null); return; }
                m_stepTask = pipeline.RunStep(index);
                if (m_stepTask.IsCompleted) CompleteStep();
            }
            catch (Exception exception) { Finish(exception); }
        }

        private static void CompleteStep()
        {
            m_stepTask.GetAwaiter().GetResult();
            m_stepTask = null;
            SessionState.SetInt(m_indexKey, SessionState.GetInt(m_indexKey, 0) + 1);
            m_waitUpdates = 2;
        }

        private static void Finish(Exception exception)
        {
            EditorApplication.update -= Tick;
            SessionState.EraseString(m_guidKey);
            SessionState.EraseInt(m_indexKey);
            m_stepTask = null;
            EditorUtility.ClearProgressBar();
            if (exception != null) Debug.LogException(exception);
            else Debug.Log("UniTools Build pipeline completed.");
            EditorApplication.Exit(exception == null ? 0 : 1);
        }
    }
}
