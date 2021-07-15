using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace UniTools.Build
{
    [Serializable]
    public class StepData
    {
        public string Name = default;
        public string StartTime = default;
        public string EndTime = default;
        public enum State { Success, Failed }
        public StepData(string name)
        {
            Name = name;
        }
    }

    public class BuildDiagnostics
    {
        private Stopwatch m_stopwatch = default;
        private List<StepData> m_buildDiagnisticsData = new List<StepData>();
        private StepData m_stepDiagnisticsData = default;

        private string m_pipelineName = default;

        //TODO: Save information about pre/post steps in 1 file
        public BuildDiagnostics(string pipelineName)
        {
            m_pipelineName = pipelineName;
        }

        public void StartTrackingStep(string name)
        {
            m_stepDiagnisticsData = new StepData(name);
            m_stepDiagnisticsData.StartTime = DateTime.Now.ToString();
            m_stopwatch = Stopwatch.StartNew();
        }

        public void StopTrackingStep()
        {
            if (m_stepDiagnisticsData == null)
            {
                return;
            }

            m_stepDiagnisticsData.EndTime = DateTime.Now.ToString();
            m_buildDiagnisticsData.Add(m_stepDiagnisticsData);

            ResetStopwatch();

            SaveStepsInfo(m_buildDiagnisticsData);
        }

        private void ResetStopwatch()
        {
            m_stopwatch.Stop();
            m_stopwatch.Reset();
        }

        private void SaveStepsInfo(List<StepData> data)
        {
            string json = JsonHelper.ToJson(data, true);
            string fileName = $"/{m_pipelineName}Diagnostics.json";
            string path = $"{Application.dataPath.Replace("/Assets", string.Empty)}/Diagnostics";
            Directory.CreateDirectory(path);

            File.WriteAllText(path + fileName, json);
        }
    }
}
