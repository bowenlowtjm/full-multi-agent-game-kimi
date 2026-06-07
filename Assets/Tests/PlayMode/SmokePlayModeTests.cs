using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Arcade.Game;

namespace Arcade.Tests
{
    [TestFixture]
    public class SmokePlayModeTests
    {
        private GameObject _stateManager;
        private GameObject _scoreManager;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            _stateManager = new GameObject("StateManager");
            _stateManager.AddComponent<GameStateManager>();

            _scoreManager = new GameObject("ScoreManager");
            _scoreManager.AddComponent<ScoreManager>();

            yield return null;
        }

        [UnityTearDown]
        public IEnumerator Teardown()
        {
            if (_stateManager != null) Object.Destroy(_stateManager);
            if (_scoreManager != null) Object.Destroy(_scoreManager);
            PlayerPrefs.DeleteAll();
            yield return null;
        }

        [UnityTest]
        public IEnumerator GameStateManager_InitializesToInit()
        {
            var gsm = _stateManager.GetComponent<GameStateManager>();
            yield return null;
            Assert.AreEqual(GameState.INIT, gsm.CurrentState);
        }

        [UnityTest]
        public IEnumerator Target_SpawnsAndExpires()
        {
            // Create a simple target
            var targetObj = new GameObject("TestTarget");
            var target = targetObj.AddComponent<Target>();

            var def = ScriptableObject.CreateInstance<TargetDefinition>();
            def.targetType = TargetType.Pop;
            def.lifetime = 0.5f;
            def.baseScore = 10;

            target.Init(def);

            bool expired = false;
            target.OnTargetExpired += (t) => expired = true;

            // Wait for expiration
            yield return new WaitForSeconds(0.6f);

            Assert.IsTrue(expired);

            Object.Destroy(targetObj);
        }

        [UnityTest]
        public IEnumerator ScoreManager_GameOver_FiresEvent()
        {
            var sm = _scoreManager.GetComponent<ScoreManager>();
            sm.ResetGame();

            bool gameOver = false;
            sm.OnGameOver += () => gameOver = true;

            // Lose all lives
            sm.RegisterMiss();
            sm.RegisterMiss();
            sm.RegisterMiss();

            yield return null;

            Assert.IsTrue(gameOver);
        }

        [UnityTest]
        public IEnumerator GameStateMachine_TransitionsWork()
        {
            var gsm = _stateManager.GetComponent<GameStateManager>();

            // Test state transitions
            gsm.GoToMainMenu();
            yield return null;
            Assert.AreEqual(GameState.MAIN_MENU, gsm.CurrentState);

            gsm.StartGame();
            yield return null;
            Assert.AreEqual(GameState.GAMEPLAY, gsm.CurrentState);

            gsm.PauseGame();
            yield return null;
            Assert.AreEqual(GameState.PAUSE, gsm.CurrentState);

            gsm.ResumeGame();
            yield return null;
            Assert.AreEqual(GameState.GAMEPLAY, gsm.CurrentState);

            gsm.GameOver();
            yield return null;
            Assert.AreEqual(GameState.GAME_OVER, gsm.CurrentState);
        }
    }
}
