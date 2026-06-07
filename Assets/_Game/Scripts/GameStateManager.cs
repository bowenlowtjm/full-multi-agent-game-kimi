using UnityEngine;

namespace Arcade.Game
{
    public enum GameState
    {
        INIT,
        MAIN_MENU,
        GAMEPLAY,
        PAUSE,
        GAME_OVER
    }

    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        [SerializeField] private GameState _currentState = GameState.INIT;
        public GameState CurrentState => _currentState;

        public delegate void StateChangeHandler(GameState newState, GameState oldState);
        public event StateChangeHandler OnStateChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            ChangeState(GameState.INIT);
        }

        public void ChangeState(GameState newState)
        {
            if (_currentState == newState) return;

            var oldState = _currentState;
            _currentState = newState;

            Debug.Log($"[GameState] {oldState} -> {newState}");
            OnStateChanged?.Invoke(newState, oldState);
        }

        public void GoToMainMenu() => ChangeState(GameState.MAIN_MENU);
        public void StartGame() => ChangeState(GameState.GAMEPLAY);
        public void PauseGame() => ChangeState(GameState.PAUSE);
        public void ResumeGame() => ChangeState(GameState.GAMEPLAY);
        public void GameOver() => ChangeState(GameState.GAME_OVER);
    }
}
