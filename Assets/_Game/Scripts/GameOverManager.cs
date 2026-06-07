using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Arcade.Game
{
    public class GameOverManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;
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
            ScoreManager.Instance?.ResetGame();
            GameStateManager.Instance?.StartGame();
            SceneLoader.Instance?.LoadScene("Gameplay");
        }

        private void OnMenuClicked()
        {
            ScoreManager.Instance?.ResetGame();
            GameStateManager.Instance?.GoToMainMenu();
            SceneLoader.Instance?.LoadScene("MainMenu");
        }
    }
}
