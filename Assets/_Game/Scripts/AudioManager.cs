using UnityEngine;

namespace Arcade.Game
{
    /// <summary>
    /// Audio manager for music and SFX with volume control.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource uiSource;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip musicLoop;
        [SerializeField] private AudioClip hitSFX;
        [SerializeField] private AudioClip missSFX;
        [SerializeField] private AudioClip comboSFX;
        [SerializeField] private AudioClip gameOverSFX;
        [SerializeField] private AudioClip highScoreSFX;
        [SerializeField] private AudioClip buttonClickSFX;

        [Header("Settings")]
        [SerializeField] private float musicVolume = 0.8f;
        [SerializeField] private float sfxVolume = 0.8f;
        [SerializeField] private bool isMuted = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Load saved settings
            LoadSettings();
        }

        private void Start()
        {
            // Start music if available
            if (musicLoop != null && musicSource != null)
            {
                PlayMusic(musicLoop);
            }
        }

        private void LoadSettings()
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
            isMuted = PlayerPrefs.GetInt("MasterMute", 0) == 1;

            ApplyVolumeSettings();
        }

        private void ApplyVolumeSettings()
        {
            float masterVolume = isMuted ? 0f : 1f;

            if (musicSource != null)
                musicSource.volume = musicVolume * masterVolume;
            if (sfxSource != null)
                sfxSource.volume = sfxVolume * masterVolume;
            if (uiSource != null)
                uiSource.volume = sfxVolume * masterVolume;
        }

        #region Volume Control

        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            if (musicSource != null && !isMuted)
                musicSource.volume = musicVolume;
        }

        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            if (sfxSource != null && !isMuted)
                sfxSource.volume = sfxVolume;
            if (uiSource != null && !isMuted)
                uiSource.volume = sfxVolume;
        }

        public void SetMuted(bool muted)
        {
            isMuted = muted;
            ApplyVolumeSettings();
        }

        #endregion

        #region Music

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (musicSource == null || clip == null) return;

            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }

        public void StopMusic()
        {
            if (musicSource != null)
                musicSource.Stop();
        }

        public void PauseMusic()
        {
            if (musicSource != null)
                musicSource.Pause();
        }

        public void ResumeMusic()
        {
            if (musicSource != null)
                musicSource.UnPause();
        }

        #endregion

        #region SFX

        public void PlayHit()
        {
            PlaySFX(hitSFX);
        }

        public void PlayMiss()
        {
            PlaySFX(missSFX);
        }

        public void PlayCombo()
        {
            PlaySFX(comboSFX);
        }

        public void PlayGameOver()
        {
            PlaySFX(gameOverSFX);
        }

        public void PlayHighScore()
        {
            PlaySFX(highScoreSFX);
        }

        public void PlayButtonClick()
        {
            PlayUI(buttonClickSFX);
        }

        private void PlaySFX(AudioClip clip)
        {
            if (sfxSource == null || clip == null || isMuted) return;
            sfxSource.PlayOneShot(clip, sfxVolume);
        }

        private void PlayUI(AudioClip clip)
        {
            if (uiSource == null || clip == null || isMuted) return;
            uiSource.PlayOneShot(clip, sfxVolume);
        }

        #endregion
    }
}
