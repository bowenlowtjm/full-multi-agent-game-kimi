using UnityEngine;
using UnityEngine.Profiling;
using System.Collections.Generic;

namespace Arcade.Game
{
    /// <summary>
    /// M4-007 Performance Pass: FPS monitoring and optimization tracking
    /// Attach to a persistent GameObject to track performance metrics
    /// </summary>
    public class PerformanceProfiler : MonoBehaviour
    {
        public static PerformanceProfiler Instance { get; private set; }

        [Header("Display")]
        [SerializeField] private bool showDebugUI = true;
        [SerializeField] private bool showDetailedStats = false;

        [Header("Performance Targets")]
        [SerializeField] private float targetFPS = 60f;
        [SerializeField] private float minAcceptableFPS = 55f;
        [SerializeField] private long maxMemoryMB = 150;

        [Header("Monitoring")]
        [SerializeField] private float sampleInterval = 1f;

        // Runtime stats
        public float CurrentFPS { get; private set; }
        public float AverageFPS { get; private set; }
        public float MinFPS { get; private set; } = float.MaxValue;
        public float MaxFPS { get; private set; }
        public long MemoryUsageMB { get; private set; }
        public int FrameDropCount { get; private set; }

        // Internal tracking
        private float fpsAccumulator = 0f;
        private int fpsFrameCount = 0;
        private float sampleTimer = 0f;
        private Queue<float> fpsHistory = new Queue<float>();
        private const int MAX_HISTORY = 60; // 1 minute at 1 sample/sec

        // Optimization flags
        public bool IsPerformanceGood => AverageFPS >= minAcceptableFPS;
        public bool IsMemoryGood => MemoryUsageMB < maxMemoryMB;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Application.targetFrameRate = Mathf.RoundToInt(targetFPS);
        }

        private void Update()
        {
            // Calculate FPS
            float currentFrameTime = Time.unscaledDeltaTime;
            float currentFPS = 1f / currentFrameTime;
            
            fpsAccumulator += currentFPS;
            fpsFrameCount++;

            // Check for frame drops
            if (currentFPS < minAcceptableFPS)
            {
                FrameDropCount++;
            }

            // Update stats at interval
            sampleTimer += Time.unscaledDeltaTime;
            if (sampleTimer >= sampleInterval)
            {
                UpdateStats();
                sampleTimer = 0f;
            }
        }

        private void UpdateStats()
        {
            // Average FPS over interval
            CurrentFPS = fpsAccumulator / fpsFrameCount;
            fpsHistory.Enqueue(CurrentFPS);
            if (fpsHistory.Count > MAX_HISTORY)
                fpsHistory.Dequeue();

            // Recalculate averages
            float sum = 0f;
            foreach (float fps in fpsHistory)
                sum += fps;
            AverageFPS = sum / fpsHistory.Count;

            // Track min/max
            if (CurrentFPS < MinFPS) MinFPS = CurrentFPS;
            if (CurrentFPS > MaxFPS) MaxFPS = CurrentFPS;

            // Memory
            MemoryUsageMB = Profiler.GetTotalAllocatedMemory(false) / (1024 * 1024);

            // Reset accumulators
            fpsAccumulator = 0f;
            fpsFrameCount = 0;
        }

        private void OnGUI()
        {
            if (!showDebugUI) return;

            float y = 10f;
            float lineHeight = 20f;
            float width = showDetailedStats ? 400f : 250f;

            // Background
            GUI.Box(new Rect(10, 10, width, showDetailedStats ? 200f : 80f), "");

            // FPS (color-coded)
            Color fpsColor = CurrentFPS >= targetFPS ? Color.green : 
                             CurrentFPS >= minAcceptableFPS ? Color.yellow : Color.red;
            GUI.color = fpsColor;
            GUI.Label(new Rect(20, y, width, lineHeight), $"FPS: {CurrentFPS:F1} / {targetFPS:F0}");
            y += lineHeight;
            GUI.color = Color.white;

            // Memory
            Color memColor = MemoryUsageMB < maxMemoryMB ? Color.green : Color.red;
            GUI.color = memColor;
            GUI.Label(new Rect(20, y, width, lineHeight), $"Memory: {MemoryUsageMB} MB / {maxMemoryMB} MB");
            y += lineHeight;
            GUI.color = Color.white;

            // Status
            string status = IsPerformanceGood && IsMemoryGood ? "✓ GOOD" : "⚠ ISSUES";
            GUI.Label(new Rect(20, y, width, lineHeight), $"Status: {status}");
            y += lineHeight;

            if (showDetailedStats)
            {
                GUI.Label(new Rect(20, y, width, lineHeight), $"Avg: {AverageFPS:F1} | Min: {MinFPS:F1} | Max: {MaxFPS:F1}");
                y += lineHeight;
                GUI.Label(new Rect(20, y, width, lineHeight), $"Frame Drops: {FrameDropCount}");
                y += lineHeight;
                GUI.Label(new Rect(20, y, width, lineHeight), $"Target: {targetFPS} FPS | Min Acceptable: {minAcceptableFPS} FPS");
            }
        }

        public void ResetStats()
        {
            fpsHistory.Clear();
            MinFPS = float.MaxValue;
            MaxFPS = 0f;
            FrameDropCount = 0;
            AverageFPS = 0f;
        }

        public PerformanceReport GenerateReport()
        {
            return new PerformanceReport
            {
                Timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                AverageFPS = AverageFPS,
                MinFPS = MinFPS,
                MaxFPS = MaxFPS,
                FrameDropCount = FrameDropCount,
                MemoryUsageMB = MemoryUsageMB,
                IsPerformanceGood = IsPerformanceGood,
                IsMemoryGood = IsMemoryGood,
                DeviceModel = SystemInfo.deviceModel,
                OS = SystemInfo.operatingSystem
            };
        }
    }

    [System.Serializable]
    public struct PerformanceReport
    {
        public string Timestamp;
        public float AverageFPS;
        public float MinFPS;
        public float MaxFPS;
        public int FrameDropCount;
        public long MemoryUsageMB;
        public bool IsPerformanceGood;
        public bool IsMemoryGood;
        public string DeviceModel;
        public string OS;

        public override string ToString()
        {
            return $"Performance Report ({Timestamp})\n" +
                   $"Device: {DeviceModel}\n" +
                   $"OS: {OS}\n" +
                   $"Average FPS: {AverageFPS:F1}\n" +
                   $"Min FPS: {MinFPS:F1}\n" +
                   $"Max FPS: {MaxFPS:F1}\n" +
                   $"Frame Drops: {FrameDropCount}\n" +
                   $"Memory: {MemoryUsageMB} MB\n" +
                   $"Status: {(IsPerformanceGood ? "PASS" : "FAIL")}";
        }
    }
}
