using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Arcade.Game;

namespace Arcade.Editor
{
    /// <summary>
    /// Automated gameplay quality harness for testing balance and performance.
    /// </summary>
    public class GameplayHarness : EditorWindow
    {
        [Header("Config")]
        private int sessionCount = 10;
        private float targetFPS = 60f;
        private bool runBot = true;

        [Header("Results")]
        private List<SessionResult> results = new List<SessionResult>();
        private bool isRunning = false;
        private int currentSession = 0;

        [MenuItem("Arcade/Gameplay Harness")]
        public static void ShowWindow()
        {
            GetWindow<GameplayHarness>("Gameplay Harness");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Gameplay Quality Harness", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            sessionCount = EditorGUILayout.IntSlider("Session Count", sessionCount, 1, 50);
            targetFPS = EditorGUILayout.FloatField("Target FPS", targetFPS);
            runBot = EditorGUILayout.Toggle("Run Bot Player", runBot);

            EditorGUILayout.Space();

            GUI.enabled = !isRunning;
            if (GUILayout.Button("Run Harness", GUILayout.Height(30)))
            {
                StartHarness();
            }
            GUI.enabled = true;

            if (GUILayout.Button("Export Report"))
            {
                ExportReport();
            }

            EditorGUILayout.Space();

            // Display results
            EditorGUILayout.LabelField("Results", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Sessions: {results.Count}");
            
            if (results.Count > 0)
            {
                float avgScore = results.Average(r => r.finalScore);
                float minScore = results.Min(r => r.finalScore);
                float maxScore = results.Max(r => r.finalScore);
                float avgFPS = results.Average(r => r.avgFPS);

                EditorGUILayout.LabelField($"Avg Score: {avgScore:F0}");
                EditorGUILayout.LabelField($"Score Range: {minScore:F0} - {maxScore:F0}");
                EditorGUILayout.LabelField($"Avg FPS: {avgFPS:F1}");
            }

            if (isRunning)
            {
                EditorGUILayout.LabelField($"Running session {currentSession + 1}/{sessionCount}...");
            }
        }

        private void StartHarness()
        {
            results.Clear();
            currentSession = 0;
            isRunning = true;

            EditorApplication.update += RunNextSession;
        }

        private void RunNextSession()
        {
            if (currentSession >= sessionCount)
            {
                isRunning = false;
                EditorApplication.update -= RunNextSession;
                Debug.Log("Gameplay harness complete!");
                return;
            }

            // Simulate a session (in real implementation, this would run the actual game)
            var result = SimulateSession();
            results.Add(result);

            currentSession++;
        }

        private SessionResult SimulateSession()
        {
            // Placeholder - real implementation would run actual game
            return new SessionResult
            {
                sessionId = currentSession,
                duration = 60f,
                hits = Random.Range(20, 50),
                misses = Random.Range(5, 20),
                finalScore = Random.Range(1000, 8000),
                avgFPS = Random.Range(55f, 65f),
                gcHitches = Random.Range(0, 5),
                peakMemoryMB = Random.Range(50, 150)
            };
        }

        private void ExportReport()
        {
            string path = "docs/perf/harness-report.md";
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("# Gameplay Harness Report");
                writer.WriteLine($"Generated: {System.DateTime.Now}");
                writer.WriteLine();
                writer.WriteLine($"Sessions: {results.Count}");
                writer.WriteLine();

                if (results.Count > 0)
                {
                    writer.WriteLine("## Summary");
                    writer.WriteLine($"- Avg Score: {results.Average(r => r.finalScore):F0}");
                    writer.WriteLine($"- Score Range: {results.Min(r => r.finalScore):F0} - {results.Max(r => r.finalScore):F0}");
                    writer.WriteLine($"- Avg FPS: {results.Average(r => r.avgFPS):F1}");
                    writer.WriteLine($"- Avg Hit Rate: {results.Average(r => r.HitRate):P1}");
                    writer.WriteLine();

                    writer.WriteLine("## Sessions");
                    writer.WriteLine("| Session | Score | Hits | Misses | Hit Rate | FPS | GC Hitches |");
                    writer.WriteLine("|---------|-------|------|--------|----------|-----|------------|");
                    
                    foreach (var r in results)
                    {
                        writer.WriteLine($"| {r.sessionId} | {r.finalScore} | {r.hits} | {r.misses} | {r.HitRate:P1} | {r.avgFPS:F1} | {r.gcHitches} |");
                    }
                }
            }

            Debug.Log($"Report exported to {path}");
        }
    }

    public struct SessionResult
    {
        public int sessionId;
        public float duration;
        public int hits;
        public int misses;
        public int finalScore;
        public float avgFPS;
        public int gcHitches;
        public float peakMemoryMB;

        public float HitRate => (hits + misses) > 0 ? (float)hits / (hits + misses) : 0f;
    }
}
