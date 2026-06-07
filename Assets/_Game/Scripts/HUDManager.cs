using UnityEngine;
using UnityEngine.UI;

namespace Arcade.Game
{
    public class HUDManager : MonoBehaviour
    {
        [Header("Score")]
        [SerializeField] private Text scoreText;

        [Header("Combo")]
        [SerializeField] private Text comboText;
        [SerializeField] private GameObject comboContainer;

        [Header("Lives")]
        [SerializeField] private Transform livesContainer;
        [SerializeField] private GameObject lifeIconPrefab;

        [Header("Pause")]
        [SerializeField] private Button pauseButton;
        [SerializeField] private GameObject pausePanel;

        [Header("High Score")]
        [SerializeField] private Text highScoreText;

        [Header("Trash Zone")]
        [SerializeField] private TrashBinZone trashBinZone;

        private int displayedLives = 0;

        private void Start()
        {
            // Subscribe to events
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnScoreChanged += OnScoreChanged;
                ScoreManager.Instance.OnComboChanged += OnComboChanged;
                ScoreManager.Instance.OnLivesChanged += OnLivesChanged;
                ScoreManager.Instance.OnGameOver += OnGameOver;
            }

            if (pauseButton != null)
                pauseButton.onClick.AddListener(TogglePause);

            // Init display
            UpdateScoreDisplay(0);
            UpdateComboDisplay(1);
            UpdateLivesDisplay(3);
            UpdateHighScoreDisplay();

            if (pausePanel != null)
                pausePanel.SetActive(false);
        }

        private void OnDestroy()
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnScoreChanged -= OnScoreChanged;
                ScoreManager.Instance.OnComboChanged -= OnComboChanged;
                ScoreManager.Instance.OnLivesChanged -= OnLivesChanged;
                ScoreManager.Instance.OnGameOver -= OnGameOver;
            }
        }

        private void OnScoreChanged(int newScore, int added)
        {
            UpdateScoreDisplay(newScore);
        }

        private void OnComboChanged(int multiplier, int hits)
        {
            UpdateComboDisplay(multiplier);
        }

        private void OnLivesChanged(int lives)
        {
            UpdateLivesDisplay(lives);
        }

        private void OnGameOver()
        {
            UpdateHighScoreDisplay();
        }

        private void UpdateScoreDisplay(int score)
        {
            if (scoreText != null)
                scoreText.text = score.ToString("D6");
        }

        private void UpdateComboDisplay(int multiplier)
        {
            if (comboContainer != null)
                comboContainer.SetActive(multiplier > 1);

            if (comboText != null)
                comboText.text = $"{multiplier}x";
        }

        private void UpdateLivesDisplay(int lives)
        {
            // Add/remove life icons
            int currentIcons = livesContainer?.childCount ?? 0;

            // Remove excess
            for (int i = currentIcons - 1; i >= lives; i--)
            {
                if (livesContainer != null && i < livesContainer.childCount)
                    Destroy(livesContainer.GetChild(i).gameObject);
            }

            // Add missing
            for (int i = currentIcons; i < lives; i++)
            {
                if (lifeIconPrefab != null && livesContainer != null)
                    Instantiate(lifeIconPrefab, livesContainer);
            }

            displayedLives = lives;
        }

        private void UpdateHighScoreDisplay()
        {
            if (highScoreText != null)
            {
                int high = ScoreManager.GetHighScore();
                highScoreText.text = $"BEST: {high:D6}";
            }
        }

        private void TogglePause()
        {
            if (GameStateManager.Instance == null) return;

            if (GameStateManager.Instance.CurrentState == GameState.GAMEPLAY)
            {
                GameStateManager.Instance.PauseGame();
                if (pausePanel != null)
                    pausePanel.SetActive(true);
                Time.timeScale = 0f;
            }
            else if (GameStateManager.Instance.CurrentState == GameState.PAUSE)
            {
                GameStateManager.Instance.ResumeGame();
                if (pausePanel != null)
                    pausePanel.SetActive(false);
                Time.timeScale = 1f;
            }
        }

        public void ResumeGame()
        {
            TogglePause();
        }
    }
}
