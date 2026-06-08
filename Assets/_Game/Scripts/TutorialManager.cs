using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arcade.Game
{
    /// <summary>
    /// Interactive tutorial for teaching the 5 gestures.
    /// Shown once on first launch, replayable from Settings.
    /// </summary>
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; }

        [Header("UI")]
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private Text instructionText;
        [SerializeField] private Text gestureNameText;
        [SerializeField] private Image gestureIcon;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button skipButton;
        [SerializeField] private Button practiceButton;
        [SerializeField] private Toggle dontShowAgainToggle;

        [Header("Steps")]
        [SerializeField] private TutorialStep[] steps;

        [Header("Settings")]
        [SerializeField] private string playerPrefKey = "TutorialShown";

        private int currentStep = 0;
        private bool isActive = false;

        [System.Serializable]
        public struct TutorialStep
        {
            public string gestureName;
            public string instruction;
            public GestureType gestureType;
            public Sprite icon;
            public bool requiresPractice;
        }

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
            // Setup buttons
            if (nextButton != null)
                nextButton.onClick.AddListener(OnNextClicked);
            if (skipButton != null)
                skipButton.onClick.AddListener(OnSkipClicked);
            if (practiceButton != null)
                practiceButton.onClick.AddListener(OnPracticeClicked);

            // Hide initially
            if (tutorialPanel != null)
                tutorialPanel.SetActive(false);
        }

        /// <summary>
        /// Check if tutorial should be shown on first launch.
        /// </summary>
        public bool ShouldShowTutorial()
        {
            return PlayerPrefs.GetInt(playerPrefKey, 0) == 0;
        }

        /// <summary>
        /// Start the tutorial sequence.
        /// </summary>
        public void StartTutorial()
        {
            if (steps == null || steps.Length == 0) return;

            isActive = true;
            currentStep = 0;

            if (tutorialPanel != null)
                tutorialPanel.SetActive(true);

            ShowStep(0);
        }

        /// <summary>
        /// Show a specific tutorial step.
        /// </summary>
        private void ShowStep(int index)
        {
            if (index < 0 || index >= steps.Length) return;

            currentStep = index;
            TutorialStep step = steps[index];

            // Update UI
            if (gestureNameText != null)
                gestureNameText.text = step.gestureName;
            if (instructionText != null)
                instructionText.text = step.instruction;
            if (gestureIcon != null)
                gestureIcon.sprite = step.icon;

            // Show/hide practice button
            if (practiceButton != null)
                practiceButton.gameObject.SetActive(step.requiresPractice);

            // Update next button text
            if (nextButton != null)
            {
                Text btnText = nextButton.GetComponentInChildren<Text>();
                if (btnText != null)
                    btnText.text = (index == steps.Length - 1) ? "Finish" : "Next";
            }
        }

        private void OnNextClicked()
        {
            if (currentStep < steps.Length - 1)
            {
                ShowStep(currentStep + 1);
            }
            else
            {
                FinishTutorial();
            }
        }

        private void OnSkipClicked()
        {
            FinishTutorial();
        }

        private void OnPracticeClicked()
        {
            // TODO: Implement practice mode for current gesture
            Debug.Log($"Practice mode for: {steps[currentStep].gestureType}");
        }

        private void FinishTutorial()
        {
            // Save "don't show again" preference
            if (dontShowAgainToggle != null && dontShowAgainToggle.isOn)
            {
                PlayerPrefs.SetInt(playerPrefKey, 1);
                PlayerPrefs.Save();
            }

            isActive = false;
            if (tutorialPanel != null)
                tutorialPanel.SetActive(false);

            // Notify listeners
            OnTutorialComplete?.Invoke();
        }

        /// <summary>
        /// Reset tutorial shown flag (for testing or Settings replay).
        /// </summary>
        public void ResetTutorialFlag()
        {
            PlayerPrefs.SetInt(playerPrefKey, 0);
            PlayerPrefs.Save();
        }

        public delegate void TutorialEventHandler();
        public event TutorialEventHandler OnTutorialComplete;

        public bool IsActive => isActive;
    }
}
