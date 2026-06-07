using NUnit.Framework;
using UnityEngine;
using Arcade.Game;

namespace Arcade.Tests
{
    [TestFixture]
    public class RulesetDefinitionTests
    {
        private RulesetDefinition _ruleset;

        [SetUp]
        public void Setup()
        {
            _ruleset = ScriptableObject.CreateInstance<RulesetDefinition>();
        }

        [TearDown]
        public void Teardown()
        {
            if (_ruleset != null)
                Object.DestroyImmediate(_ruleset);
        }

        [Test]
        public void Ruleset_Creation_NotNull()
        {
            Assert.IsNotNull(_ruleset);
        }

        [Test]
        public void Ruleset_DefaultSeed_IsZero()
        {
            Assert.AreEqual(0, _ruleset.seed);
        }

        [Test]
        public void Ruleset_DefaultLives_IsThree()
        {
            Assert.AreEqual(3, _ruleset.lives);
        }

        [Test]
        public void Ruleset_DefaultComboCap_IsFive()
        {
            Assert.AreEqual(5f, _ruleset.comboCap);
        }

        [Test]
        public void Ruleset_DefaultTargetLifetime_IsFive()
        {
            Assert.AreEqual(5f, _ruleset.targetLifetime);
        }

        [Test]
        public void Ruleset_DefaultMaxConcurrent_IsFour()
        {
            Assert.AreEqual(4, _ruleset.maxConcurrentTargets);
        }

        [Test]
        public void Ruleset_SeededRNG_ProducesIdenticalSequence()
        {
            // Arrange
            int seed = 12345;
            var rng1 = new System.Random(seed);
            var rng2 = new System.Random(seed);

            // Act & Assert
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(rng1.Next(), rng2.Next());
            }
        }

        [Test]
        public void Ruleset_DifferentSeeds_ProduceDifferentSequences()
        {
            // Arrange
            var rng1 = new System.Random(11111);
            var rng2 = new System.Random(22222);

            // Act & Assert
            bool allSame = true;
            for (int i = 0; i < 10; i++)
            {
                if (rng1.Next() != rng2.Next())
                {
                    allSame = false;
                    break;
                }
            }
            Assert.IsFalse(allSame);
        }

        [Test]
        public void Ruleset_SpawnInterval_IsValid()
        {
            // Arrange
            _ruleset.spawnIntervalStart = 1.5f;
            _ruleset.spawnIntervalEnd = 0.4f;

            // Assert
            Assert.Greater(_ruleset.spawnIntervalStart, _ruleset.spawnIntervalEnd);
            Assert.Greater(_ruleset.spawnIntervalStart, 0);
            Assert.Greater(_ruleset.spawnIntervalEnd, 0);
        }

        [Test]
        public void Ruleset_ComboCap_IsPositive()
        {
            // Arrange
            _ruleset.comboCap = 10f;

            // Assert
            Assert.Greater(_ruleset.comboCap, 0);
        }
    }
}
