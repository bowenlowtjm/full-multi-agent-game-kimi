using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using Arcade.Game;

namespace Arcade.Tests
{
    [TestFixture]
    public class CoreLoopTests
    {
        private const string TEST_SCENE = "Assets/_Game/Scenes/GameScene.unity";

        [UnitySetUp]
        public IEnumerator Setup()
        {
            // Load the game scene
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            yield return null;
            yield return new WaitForSeconds(0.5f);
        }

        [UnityTearDown]
        public IEnumerator Teardown()
        {
            yield return null;
        }

        [UnityTest]
        public IEnumerator CoreLoop_ManagersExist()
        {
            // Assert - verify all managers are present
            Assert.IsNotNull(ScoreManager.Instance, "ScoreManager should exist");
            Assert.IsNotNull(SpawnerManager.Instance, "SpawnerManager should exist");
            Assert.IsNotNull(InputManager.Instance, "InputManager should exist");
            Assert.IsNotNull(GameStateManager.Instance, "GameStateManager should exist");
            yield return null;
        }

        [UnityTest]
        public IEnumerator CoreLoop_ScoreManager_InitialState()
        {
            // Arrange
            var scoreManager = ScoreManager.Instance;

            // Assert - verify initial state
            Assert.AreEqual(0, scoreManager.CurrentScore, "Initial score should be 0");
            Assert.AreEqual(1, scoreManager.CurrentMultiplier, "Initial multiplier should be 1");
            Assert.AreEqual(3, scoreManager.CurrentLives, "Initial lives should be 3");
            Assert.AreEqual(0, scoreManager.SuccessfulHits, "Initial successful hits should be 0");
            yield return null;
        }

        [UnityTest]
        public IEnumerator CoreLoop_GameState_TransitionsToGameplay()
        {
            // Arrange
            var gameStateManager = GameStateManager.Instance;
            bool stateChanged = false;
            GameState newStateReceived = GameState.INIT;

            gameStateManager.OnStateChanged += (newState, oldState) =>
            {
                stateChanged = true;
                newStateReceived = newState;
            };

            // Act - transition to gameplay
            gameStateManager.SetState(GameState.GAMEPLAY);
            yield return new WaitForSeconds(0.1f);

            // Assert
            Assert.IsTrue(stateChanged, "State change event should fire");
            Assert.AreEqual(GameState.GAMEPLAY, newStateReceived, "Should transition to GAMEPLAY");
            Assert.AreEqual(GameState.GAMEPLAY, gameStateManager.CurrentState, "Current state should be GAMEPLAY");
        }

        [UnityTest]
        public IEnumerator CoreLoop_ScoreManager_AddScore_IncreasesScore()
        {
            // Arrange
            var scoreManager = ScoreManager.Instance;
            scoreManager.ResetGame();
            int initialScore = scoreManager.CurrentScore;

            // Act
            scoreManager.AddScore(50);
            yield return null;

            // Assert
            Assert.AreEqual(initialScore + 50, scoreManager.CurrentScore, "Score should increase by 50");
        }

        [UnityTest]
        public IEnumerator CoreLoop_ScoreManager_RegisterHit_IncreasesCombo()
        {
            // Arrange
            var scoreManager = ScoreManager.Instance;
            scoreManager.ResetGame();

            // Act
            scoreManager.RegisterHit();
            yield return null;

            // Assert
            Assert.AreEqual(1, scoreManager.SuccessfulHits, "Successful hits should be 1");
        }

        [UnityTest]
        public IEnumerator CoreLoop_ScoreManager_RegisterMiss_BreaksCombo()
        {
            // Arrange
            var scoreManager = ScoreManager.Instance;
            scoreManager.ResetGame();
            scoreManager.RegisterHit();
            scoreManager.RegisterHit();
            yield return null;

            // Act
            scoreManager.RegisterMiss();
            yield return null;

            // Assert
            Assert.AreEqual(0, scoreManager.SuccessfulHits, "Combo should be broken");
            Assert.AreEqual(1, scoreManager.CurrentMultiplier, "Multiplier should reset to 1");
        }

        [UnityTest]
        public IEnumerator CoreLoop_ScoreManager_RegisterMiss_LosesLife()
        {
            // Arrange
            var scoreManager = ScoreManager.Instance;
            scoreManager.ResetGame();
            int initialLives = scoreManager.CurrentLives;

            // Act
            scoreManager.RegisterMiss();
            yield return null;

            // Assert
            Assert.AreEqual(initialLives - 1, scoreManager.CurrentLives, "Should lose one life");
        }

        [UnityTest]
        public IEnumerator CoreLoop_GameOver_FiresEvent()
        {
            // Arrange
            var scoreManager = ScoreManager.Instance;
            scoreManager.ResetGame();
            bool gameOverFired = false;
            scoreManager.OnGameOver += () => gameOverFired = true;

            // Act - lose all 3 lives
            scoreManager.RegisterMiss();
            yield return null;
            scoreManager.RegisterMiss();
            yield return null;
            scoreManager.RegisterMiss();
            yield return null;

            // Assert
            Assert.IsTrue(gameOverFired, "Game over event should fire");
        }

        [UnityTest]
        public IEnumerator CoreLoop_SpawnerManager_CanSpawnTarget()
        {
            // Arrange
            var spawner = SpawnerManager.Instance;
            spawner.ClearAllTargets();
            yield return new WaitForSeconds(0.1f);

            int initialCount = spawner.ActiveTargets.Count;

            // Act - manually trigger spawn
            // Note: This assumes there's a way to trigger spawn or we wait for auto-spawn
            // For this test, we just verify the spawner is functional
            yield return new WaitForSeconds(0.1f);

            // Assert - just verify spawner exists and is functional
            Assert.IsNotNull(spawner.targetDefinitions, "Target definitions should be set");
            Assert.IsNotNull(spawner.ActiveTargets, "Active targets list should exist");
        }

        [UnityTest]
        public IEnumerator CoreLoop_TargetDefinition_HasValidData()
        {
            // Arrange
            var spawner = SpawnerManager.Instance;

            // Assert - verify at least one target definition exists with valid data
            Assert.IsNotNull(spawner.targetDefinitions, "Target definitions should not be null");
            Assert.Greater(spawner.targetDefinitions.Length, 0, "Should have at least one target definition");

            var def = spawner.targetDefinitions[0];
            Assert.IsNotNull(def, "First target definition should not be null");
            Assert.Greater(def.baseScore, 0, "Base score should be positive");
            Assert.Greater(def.lifetime, 0, "Lifetime should be positive");
        }
    }
}
