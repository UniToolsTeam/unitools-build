using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UniTools.Build
{
    [Serializable]
    public class StepData
    {
        public string Name = default;
        public string StartTime = default;
        public string EndTime = default;
        public double Duration = default;
        public StepData(string name)
        {
            Name = name;
        }
    }

    public class BuildDiagnostics
    {
        private Stopwatch m_stopwatch = default;
        private List<StepData> m_diagnisticsData = new List<StepData>();
        private StepData m_stepDiagnisticsData = default;

        private BuildPipeline m_pipeline = default;
        private string m_pipelineName = default;

        //TODO: Save information about pre/post steps in 1 file
        public BuildDiagnostics(BuildPipeline pipeline)
        {
            m_pipeline = pipeline;
        }

        public BuildDiagnostics(string pipelineName)
        {
            m_pipelineName = pipelineName;
        }

        //TODO: Normalize time format
        public void StartTracking(string name)
        {
            m_stepDiagnisticsData = new StepData(name);
            m_stepDiagnisticsData.StartTime = DateTime.Now.ToString();
            m_stopwatch = Stopwatch.StartNew();
        }

        public void Stop()
        {
            if (m_stepDiagnisticsData != null)
            {
                DateTime endTime = DateTime.Now;
                m_stepDiagnisticsData.EndTime = endTime.ToString();

                m_stopwatch.Stop();
                m_stopwatch.Reset();

                DateTime startTime = DateTime.Parse(m_stepDiagnisticsData.StartTime);
                m_stepDiagnisticsData.Duration = endTime.Subtract(startTime).TotalMilliseconds;
                m_diagnisticsData.Add(m_stepDiagnisticsData);

                SaveToFile(m_diagnisticsData);
            }
        }

        //TODO: Select correct location path 
        public void SaveToFile(List<StepData> data)
        {
            string json = JsonHelper.ToJson(data, true);
            string fileName = $"/{m_pipelineName}Diagnostics.json";

            System.IO.File.WriteAllText(Application.dataPath + fileName, json);
        }
    }
}
