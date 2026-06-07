using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Arcade.Game
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Button startButton;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TextMeshProUGUI versionText;

        private void Start()
        {
            if (startButton != null)
                startButton.onClick.AddListener(OnStartClicked);

            UpdateHighScoreDisplay();

            if (versionText != null)
                versionText.text = $"v{Application.version}";

            // Ensure state is MAIN_MENU
            if (GameStateManager.Instance != null)
                GameStateManager.Instance.GoToMainMenu();
        }

        private void UpdateHighScoreDisplay()
        {
            if (highScoreText != null)
            {
                int high = ScoreManager.GetHighScore();
                highScoreText.text = $"HIGH SCORE\n{high:D6}";
            }
        }

        private void OnStartClicked()
        {
            GameStateManager.Instance?.StartGame();
            SceneLoader.Instance?.LoadScene("Gameplay");
        }
    }
}
