using NUnit.Framework;
using UnityEngine;
using Arcade.Game;

namespace Arcade.Tests
{
    [TestFixture]
    public class ScoreCalculatorTests
    {
        private GameObject _scoreManagerObj;
        private ScoreManager _scoreManager;

        [SetUp]
        public void Setup()
        {
            _scoreManagerObj = new GameObject("TestScoreManager");
            _scoreManager = _scoreManagerObj.AddComponent<ScoreManager>();
        }

        [TearDown]
        public void Teardown()
        {
            if (_scoreManagerObj != null)
                Object.DestroyImmediate(_scoreManagerObj);
            PlayerPrefs.DeleteAll();
        }

        [Test]
        public void AddScore_AppliesMultiplier()
        {
            // Arrange
            _scoreManager.ResetGame();

            // Act - simulate 5 hits to reach 2x multiplier
            for (int i = 0; i < 5; i++)
            {
                _scoreManager.AddScore(10);
                _scoreManager.RegisterHit();
            }

            // Assert - 5 hits at 10pts each with multipliers:
            // 1: 10*1=10, 2: 10*1=10, 3: 10*1=10, 4: 10*1=10, 5: 10*1=10
            // Next hit should be 10*2=20
            _scoreManager.AddScore(10);
            int expected = 10 + 10 + 10 + 10 + 10 + 20;
            Assert.AreEqual(_scoreManager.CurrentScore, expected);
        }

        [Test]
        public void RegisterMiss_BreaksComboAndLosesLife()
        {
            // Arrange
            _scoreManager.ResetGame();
            _scoreManager.RegisterHit();
            _scoreManager.RegisterHit();

            // Act
            _scoreManager.RegisterMiss();

            // Assert
            Assert.AreEqual(_scoreManager.CurrentMultiplier, 1);
            Assert.AreEqual(_scoreManager.CurrentLives, 2);
        }

        [Test]
        public void BreakCombo_ResetsMultiplierAndHits()
        {
            // Arrange
            _scoreManager.ResetGame();
            _scoreManager.RegisterHit();
            _scoreManager.RegisterHit();
            _scoreManager.RegisterHit();
            _scoreManager.RegisterHit();
            _scoreManager.RegisterHit(); // Now at 2x

            int hitsBefore = _scoreManager.SuccessfulHits;

            // Act
            _scoreManager.BreakCombo();

            // Assert
            Assert.AreEqual(_scoreManager.CurrentMultiplier, 1);
            Assert.AreEqual(_scoreManager.SuccessfulHits, 0);
        }

        [Test]
        public void ComboMultiplier_CapsAtMax()
        {
            // Arrange
            _scoreManager.ResetGame();

            // Act - simulate many hits (25 = 5x multiplier)
            for (int i = 0; i < 30; i++)
            {
                _scoreManager.RegisterHit();
            }

            // Assert - should cap at 5x
            Assert.AreEqual(_scoreManager.CurrentMultiplier, 5);
        }

        [Test]
        public void Lives_GoToZero_TriggersGameOver()
        {
            // Arrange
            _scoreManager.ResetGame();
            bool gameOverFired = false;
            _scoreManager.OnGameOver += () => gameOverFired = true;

            // Act - lose all lives
            _scoreManager.RegisterMiss();
            _scoreManager.RegisterMiss();
            _scoreManager.RegisterMiss();

            // Assert
            Assert.IsTrue(gameOverFired);
            Assert.AreEqual(_scoreManager.CurrentLives, 0);
        }

        [Test]
        public void SaveHighScore_OnlyWhenHigher()
        {
            // Arrange
            PlayerPrefs.SetInt("ArcadeHighScore", 100);
            _scoreManager.ResetGame();

            // Act - add 50 points
            _scoreManager.AddScore(50);
            _scoreManager.SaveHighScore();

            // Assert - should not change
            Assert.AreEqual(ScoreManager.GetHighScore(), 100);

            // Act - beat high score
            _scoreManager.ResetGame();
            for (int i = 0; i < 20; i++)
            {
                _scoreManager.AddScore(10);
                _scoreManager.RegisterHit();
            }
            _scoreManager.SaveHighScore();

            // Assert - should update
            Assert.Greater(ScoreManager.GetHighScore(), 100);
        }

        [Test]
        public void ResetGame_ClearsAllState()
        {
            // Arrange - build up state
            _scoreManager.ResetGame();
            _scoreManager.AddScore(100);
            _scoreManager.RegisterHit();
            _scoreManager.RegisterHit();
            _scoreManager.RegisterHit();

            // Act
            _scoreManager.ResetGame();

            // Assert
            Assert.AreEqual(_scoreManager.CurrentScore, 0);
            Assert.AreEqual(_scoreManager.CurrentMultiplier, 1);
            Assert.AreEqual(_scoreManager.CurrentLives, 3);
            Assert.AreEqual(_scoreManager.SuccessfulHits, 0);
        }
    }
}
