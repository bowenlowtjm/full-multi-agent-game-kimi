using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arcade.Game
{
    /// <summary>
    /// Performance monitoring and optimization manager.
    /// Tracks FPS, memory, and provides quality settings.
    /// </summary>
    public class PerformanceManager : MonoBehaviour
    {
        public static PerformanceManager Instance { get; private set; }

        [Header("FPS Monitoring")]
        [SerializeField] private bool showFPS = false;
        [SerializeField] private Text fpsText;
        [SerializeField] private float fpsUpdateInterval = 0.5f;

        [Header("Quality Settings")]
        [SerializeField] private int targetFrameRate = 60;
        [SerializeField] private bool vsyncEnabled = false;

        [Header("Memory")]
        [SerializeField] private long maxMemoryMB = 150;

        private float fpsTimer = 0f;
        private int frameCount = 0;
        private float currentFPS = 0f;
        private int qualityLevel = 0;

        public float CurrentFPS => currentFPS;
        public bool IsPerformanceGood => currentFPS >= targetFrameRate * 0.9f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // Set target frame rate
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = vsyncEnabled ? 1 : 0;
        }

        private void Update()
        {
            // FPS calculation
            frameCount++;
            fpsTimer += Time.deltaTime;

            if (fpsTimer >= fpsUpdateInterval)
            {
                currentFPS = frameCount / fpsTimer;
                frameCount = 0;
                fpsTimer = 0f;

                if (showFPS && fpsText != null)
                {
                    fpsText.text = $"FPS: {currentFPS:F0}\nMEM: {GetMemoryMB():F0}MB";
                }

                // Auto-adjust quality if needed
                CheckPerformance();
            }
        }

        private void CheckPerformance()
        {
            // If FPS drops too low, reduce quality
            if (currentFPS < targetFrameRate * 0.5f && qualityLevel > 0)
            {
                qualityLevel--;
                ApplyQualityLevel();
            }
            // If FPS is great, can increase quality
            else if (currentFPS >= targetFrameRate && qualityLevel < QualitySettings.names.Length - 1)
            {
                qualityLevel++;
                ApplyQualityLevel();
            }
        }

        private void ApplyQualityLevel()
        {
            QualitySettings.SetQualityLevel(qualityLevel, true);
            Debug.Log($"[Performance] Quality level set to: {QualitySettings.names[qualityLevel]}");
        }

        public float GetMemoryMB()
        {
            return GC.GetTotalMemory(false) / (1024f * 1024f);
        }

        public void RequestGarbageCollection()
        {
            System.GC.Collect();
            Resources.UnloadUnusedAssets();
        }

        public void SetShowFPS(bool show)
        {
            showFPS = show;
            if (fpsText != null)
                fpsText.gameObject.SetActive(show);
        }

        public void SetTargetFrameRate(int fps)
        {
            targetFrameRate = fps;
            Application.targetFrameRate = fps;
        }
    }
}
