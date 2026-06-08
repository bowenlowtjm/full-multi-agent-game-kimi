using UnityEngine;

namespace Arcade.Game
{
    /// <summary>
    /// Mobile haptic feedback manager for hit/miss interactions.
    /// </summary>
    public class HapticsManager : MonoBehaviour
    {
        public static HapticsManager Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private bool hapticsEnabled = true;
        [SerializeField] private bool fallbackToVibration = true;

        [Header("Hit Feedback")]
        [SerializeField] private float hitDuration = 0.05f;
        [SerializeField] private float hitAmplitude = 0.5f;

        [Header("Miss Feedback")]
        [SerializeField] private float missDuration = 0.1f;
        [SerializeField] private float missAmplitude = 0.8f;

        private bool supportsHaptics = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CheckHapticSupport();
        }

        private void Start()
        {
            // Load settings
            hapticsEnabled = PlayerPrefs.GetInt("HapticsEnabled", 1) == 1;
        }

        private void CheckHapticSupport()
        {
            #if UNITY_ANDROID || UNITY_IOS
            supportsHaptics = true;
            #else
            supportsHaptics = false;
            #endif
        }

        public void SetHapticsEnabled(bool enabled)
        {
            hapticsEnabled = enabled;
            PlayerPrefs.SetInt("HapticsEnabled", enabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        public bool IsHapticsEnabled()
        {
            return hapticsEnabled;
        }

        /// <summary>
        /// Trigger haptic feedback for a successful hit.
        /// </summary>
        public void PlayHitHaptic()
        {
            if (!hapticsEnabled) return;

            #if UNITY_ANDROID
            if (supportsHaptics)
            {
                // Light impact on Android
                Handheld.Vibrate();
            }
            #elif UNITY_IOS
            // iOS light impact
            HapticFeedback.Perform(HapticFeedbackType.LightImpact);
            #else
            if (fallbackToVibration)
            {
                Handheld.Vibrate();
            }
            #endif
        }

        /// <summary>
        /// Trigger haptic feedback for a miss.
        /// </summary>
        public void PlayMissHaptic()
        {
            if (!hapticsEnabled) return;

            #if UNITY_ANDROID
            if (supportsHaptics)
            {
                // Double vibration for miss
                Handheld.Vibrate();
            }
            #elif UNITY_IOS
            // iOS heavy impact for miss
            HapticFeedback.Perform(HapticFeedbackType.HeavyImpact);
            #else
            if (fallbackToVibration)
            {
                Handheld.Vibrate();
            }
            #endif
        }

        /// <summary>
        /// Trigger haptic feedback for combo milestone.
        /// </summary>
        public void PlayComboHaptic(int comboLevel)
        {
            if (!hapticsEnabled) return;

            // Stronger feedback for higher combos
            if (comboLevel >= 5)
            {
                #if UNITY_ANDROID
                Handheld.Vibrate();
                #elif UNITY_IOS
                HapticFeedback.Perform(HapticFeedbackType.MediumImpact);
                #endif
            }
        }

        /// <summary>
        /// Trigger haptic feedback for button presses.
        /// </summary>
        public void PlayButtonHaptic()
        {
            if (!hapticsEnabled) return;

            #if UNITY_IOS
            HapticFeedback.Perform(HapticFeedbackType.Selection);
            #endif
        }
    }

    #if UNITY_IOS
    public enum HapticFeedbackType
    {
        LightImpact,
        MediumImpact,
        HeavyImpact,
        Selection
    }

    public static class HapticFeedback
    {
        public static void Perform(HapticFeedbackType type)
        {
            // iOS native haptics would be called here via Unity.iOS.Notification
            // For now, this is a placeholder for the iOS implementation
        }
    }
    #endif
}
