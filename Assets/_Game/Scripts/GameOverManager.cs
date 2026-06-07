using UnityEngine;
using UnityEngine.UI;

namespace Arcade.Game
{
    public class GameOverManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Text scoreText;
        [SerializeField] private Text highScoreText;
        [SerializeField] private GameObject newHighScoreBadge;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button menuButton;

        private void Start()
        {
            if (retryButton != null)
                retryButton.onClick.AddListener(OnRetryClicked);

            if (menuButton != null)
                menuButton.onClick.AddListener(OnMenuClicked);

            UpdateScoreDisplay();
        }

        private void UpdateScoreDisplay()
        {
            int currentScore = ScoreManager.Instance?.CurrentScore ?? 0;
            int highScore = ScoreManager.GetHighScore();

            if (scoreText != null)
                scoreText.text = $"SCORE\n{currentScore:D6}";

            if (highScoreText != null)
                highScoreText.text = $"BEST\n{highScore:D6}";

            if (newHighScoreBadge != null)
                newHighScoreBadge.SetActive(currentScore >= highScore && currentScore > 0);
        }

        private void OnRetryClicked()
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.ResetGame();
            if (GameStateManager.Instance != null)
                GameStateManager.Instance.StartGame();
            if (SceneLoader.Instance != null)
                SceneLoader.Instance.LoadScene("Gameplay");
        }

        private void OnMenuClicked()
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.ResetGame();
            if (GameStateManager.Instance != null)
                GameStateManager.Instance.GoToMainMenu();
            if (SceneLoader.Instance != null)
                SceneLoader.Instance.LoadScene("MainMenu");
        }
    }
}
