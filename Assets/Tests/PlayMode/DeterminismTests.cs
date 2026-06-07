using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using Arcade.Game;

namespace Arcade.Tests
{
    [TestFixture]
    public class DeterminismTests
    {
        [UnityTest]
        public IEnumerator Determinism_SameSeed_SameSpawns()
        {
            // Arrange - Run 1
            var spawner1 = CreateTestSpawner(12345);
            var sequence1 = new System.Collections.Generic.List<int>();
            
            // Generate 10 spawn indices
            for (int i = 0; i < 10; i++)
            {
                sequence1.Add(spawner1.SeededRng.Next(0, 100));
            }

            yield return null;

            // Arrange - Run 2 with same seed
            var spawner2 = CreateTestSpawner(12345);
            var sequence2 = new System.Collections.Generic.List<int>();
            
            for (int i = 0; i < 10; i++)
            {
                sequence2.Add(spawner2.SeededRng.Next(0, 100));
            }

            // Assert - sequences should be identical
            Assert.AreEqual(sequence1.Count, sequence2.Count, "Sequence lengths should match");
            for (int i = 0; i < sequence1.Count; i++)
            {
                Assert.AreEqual(sequence1[i], sequence2[i], $"Spawn sequence mismatch at index {i}");
            }
        }

        [UnityTest]
        public IEnumerator Determinism_DifferentSeeds_DifferentSpawns()
        {
            // Arrange - Different seeds
            var spawner1 = CreateTestSpawner(11111);
            var spawner2 = CreateTestSpawner(22222);
            
            int matchCount = 0;
            for (int i = 0; i < 20; i++)
            {
                if (spawner1.SeededRng.Next(0, 100) == spawner2.SeededRng.Next(0, 100))
                    matchCount++;
            }

            yield return null;

            // Assert - should have few or no matches (extremely unlikely to match all)
            Assert.Less(matchCount, 20, "Different seeds should produce different sequences");
        }

        [UnityTest]
        public IEnumerator Determinism_Ruleset_SeededRNG_ProducesDeterministicResults()
        {
            // Arrange
            var ruleset = ScriptableObject.CreateInstance<RulesetDefinition>();
            ruleset.seed = 12345;
            var rng1 = new System.Random(ruleset.seed);
            var rng2 = new System.Random(ruleset.seed);

            yield return null;

            // Act & Assert - 50 random values should match
            for (int i = 0; i < 50; i++)
            {
                int v1 = rng1.Next();
                int v2 = rng2.Next();
                Assert.AreEqual(v1, v2, $"RNG mismatch at iteration {i}");
            }

            Object.DestroyImmediate(ruleset);
        }

        [UnityTest]
        public IEnumerator Determinism_ScoreManager_ComboCalculation_IsDeterministic()
        {
            // Arrange
            var go = new GameObject("TestScoreManager");
            var scoreManager = go.AddComponent<ScoreManager>();
            scoreManager.ResetGame();

            yield return null;

            // Act - simulate deterministic inputs
            int[] scores = new int[10];
            for (int i = 0; i < 10; i++)
            {
                scoreManager.AddScore(100);
                scoreManager.RegisterHit();
                scores[i] = scoreManager.CurrentScore;
            }

            // Reset and replay
            scoreManager.ResetGame();
            int[] replayScores = new int[10];
            for (int i = 0; i < 10; i++)
            {
                scoreManager.AddScore(100);
                scoreManager.RegisterHit();
                replayScores[i] = scoreManager.CurrentScore;
            }

            // Assert
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(scores[i], replayScores[i], $"Score mismatch at iteration {i}");
            }

            Object.DestroyImmediate(go);
        }

        private SpawnerManager CreateTestSpawner(int seed)
        {
            var go = new GameObject("TestSpawner");
            var spawner = go.AddComponent<SpawnerManager>();
            
            var ruleset = ScriptableObject.CreateInstance<RulesetDefinition>();
            ruleset.seed = seed;
            spawner.SetRuleset(ruleset);
            
            return spawner;
        }
    }
}
