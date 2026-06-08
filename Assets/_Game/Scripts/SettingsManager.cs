using UnityEngine;
using UnityEngine.UI;

namespace Arcade.Game
{
    /// <summary>
    /// Settings screen manager for audio, haptics, and tutorial.
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { get; private set; }

        [Header("Audio")]
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Toggle muteToggle;

        [Header("Haptics")]
        [SerializeField] private Toggle hapticsToggle;

        [Header("Tutorial")]
        [SerializeField] private Button replayTutorialButton;

        [Header("Navigation")]
        [SerializeField] private Button backButton;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            LoadSettings();
            SetupListeners();
        }

        private void SetupListeners()
        {
            // Audio
            if (musicVolumeSlider != null)
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            if (muteToggle != null)
                muteToggle.onValueChanged.AddListener(OnMuteToggled);

            // Haptics
            if (hapticsToggle != null)
                hapticsToggle.onValueChanged.AddListener(OnHapticsToggled);

            // Tutorial
            if (replayTutorialButton != null)
                replayTutorialButton.onClick.AddListener(OnReplayTutorial);

            // Navigation
            if (backButton != null)
                backButton.onClick.AddListener(OnBackClicked);
        }

        private void LoadSettings()
        {
            // Audio
            if (musicVolumeSlider != null)
                musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
            if (muteToggle != null)
                muteToggle.isOn = PlayerPrefs.GetInt("MasterMute", 0) == 1;

            // Haptics
            if (hapticsToggle != null)
                hapticsToggle.isOn = PlayerPrefs.GetInt("HapticsEnabled", 1) == 1;
        }

        private void OnMusicVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save();
            AudioManager.Instance?.SetMusicVolume(value);
        }

        private void OnSFXVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
            PlayerPrefs.Save();
            AudioManager.Instance?.SetSFXVolume(value);
        }

        private void OnMuteToggled(bool isMuted)
        {
            PlayerPrefs.SetInt("MasterMute", isMuted ? 1 : 0);
            PlayerPrefs.Save();
            AudioManager.Instance?.SetMuted(isMuted);
        }

        private void OnHapticsToggled(bool enabled)
        {
            HapticsManager.Instance?.SetHapticsEnabled(enabled);
        }

        private void OnReplayTutorial()
        {
            TutorialManager.Instance?.ResetTutorialFlag();
            TutorialManager.Instance?.StartTutorial();
        }

        private void OnBackClicked()
        {
            // Return to previous screen
            gameObject.SetActive(false);
        }
    }
}
