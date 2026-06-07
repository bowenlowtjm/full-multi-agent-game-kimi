using UnityEngine;

namespace Arcade.Game
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        [Header("Score")]
        [SerializeField] private int _currentScore = 0;
        public int CurrentScore => _currentScore;

        [Header("Combo")]
        [SerializeField] private int _successfulHits = 0;
        [SerializeField] private int _currentMultiplier = 1;
        public int CurrentMultiplier => _currentMultiplier;
        public int SuccessfulHits => _successfulHits;

        [Header("Config")]
        [SerializeField] private int _hitsPerMultiplierStep = 5;
        [SerializeField] private int _maxMultiplier = 5;

        [Header("Lives")]
        [SerializeField] private int _maxLives = 3;
        [SerializeField] private int _currentLives;
        public int CurrentLives => _currentLives;
        public int MaxLives => _maxLives;

        [Header("High Score")]
        private const string HIGH_SCORE_KEY = "ArcadeHighScore";

        public delegate void ScoreChangeHandler(int newScore, int addedPoints);
        public delegate void ComboChangeHandler(int newMultiplier, int hits);
        public delegate void LivesChangeHandler(int newLives);
        public event ScoreChangeHandler OnScoreChanged;
        public event ComboChangeHandler OnComboChanged;
        public event LivesChangeHandler OnLivesChanged;
        public event System.Action OnComboBroken;
        public event System.Action OnGameOver;

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
            ResetGame();
        }

        public void ResetGame()
        {
            _currentScore = 0;
            _successfulHits = 0;
            _currentMultiplier = 1;
            _currentLives = _maxLives;
            OnScoreChanged?.Invoke(0, 0);
            OnComboChanged?.Invoke(1, 0);
            OnLivesChanged?.Invoke(_currentLives);
        }

        public void AddScore(int basePoints)
        {
            int finalPoints = basePoints * _currentMultiplier;
            _currentScore += finalPoints;
            OnScoreChanged?.Invoke(_currentScore, finalPoints);
        }

        public void RegisterHit()
        {
            _successfulHits++;
            UpdateMultiplier();
            OnComboChanged?.Invoke(_currentMultiplier, _successfulHits);
        }

        public void RegisterMiss()
        {
            BreakCombo();
            LoseLife();
        }

        public void BreakCombo()
        {
            if (_successfulHits > 0)
            {
                _successfulHits = 0;
                _currentMultiplier = 1;
                OnComboBroken?.Invoke();
                OnComboChanged?.Invoke(1, 0);
            }
        }

        private void UpdateMultiplier()
        {
            int newMultiplier = 1 + (_successfulHits / _hitsPerMultiplierStep);
            _currentMultiplier = Mathf.Min(newMultiplier, _maxMultiplier);
        }

        private void LoseLife()
        {
            _currentLives--;
            OnLivesChanged?.Invoke(_currentLives);

            if (_currentLives <= 0)
            {
                SaveHighScore();
                OnGameOver?.Invoke();
            }
        }

        public void SaveHighScore()
        {
            int savedHigh = GetHighScore();
            if (_currentScore > savedHigh)
            {
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, _currentScore);
                PlayerPrefs.Save();
            }
        }

        public static int GetHighScore()
        {
            return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        }
    }
}
