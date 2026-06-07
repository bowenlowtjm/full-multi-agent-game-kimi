using NUnit.Framework;
using UnityEngine;
using Arcade.Game;

namespace Arcade.Tests
{
    [TestFixture]
    public class GestureRecognizerTests
    {
        private GestureRecognizer _recognizer;

        [SetUp]
        public void Setup()
        {
            _recognizer = new GestureRecognizer();
        }

        [Test]
        public void Recognizer_Creation_NotNull()
        {
            Assert.IsNotNull(_recognizer);
        }

        [Test]
        public void Recognizer_DefaultThresholds_AreSet()
        {
            Assert.Greater(_recognizer.tapTimeThreshold, 0);
            Assert.Greater(_recognizer.doubleTapInterval, 0);
            Assert.Greater(_recognizer.longPressDuration, 0);
            Assert.Greater(_recognizer.swipeThreshold, 0);
            Assert.Greater(_recognizer.twoFingerMaxDistance, 0);
        }

        [Test]
        public void Recognizer_SetThresholds_Works()
        {
            // Act
            _recognizer.tapTimeThreshold = 0.2f;
            _recognizer.doubleTapInterval = 0.3f;
            _recognizer.longPressDuration = 0.8f;
            _recognizer.swipeThreshold = 50f;
            _recognizer.twoFingerMaxDistance = 100f;

            // Assert
            Assert.AreEqual(0.2f, _recognizer.tapTimeThreshold);
            Assert.AreEqual(0.3f, _recognizer.doubleTapInterval);
            Assert.AreEqual(0.8f, _recognizer.longPressDuration);
            Assert.AreEqual(50f, _recognizer.swipeThreshold);
            Assert.AreEqual(100f, _recognizer.twoFingerMaxDistance);
        }

        [Test]
        public void Recognizer_GestureTypeEnum_HasExpectedValues()
        {
            // Verify enum values match expected gesture types
            Assert.AreEqual(0, (int)GestureType.SingleTap);
            Assert.AreEqual(1, (int)GestureType.DoubleTap);
            Assert.AreEqual(2, (int)GestureType.LongPress);
            Assert.AreEqual(3, (int)GestureType.SwipeTap);
            Assert.AreEqual(4, (int)GestureType.TwoFingerTap);
        }

        [Test]
        public void Recognizer_TapData_StoresCorrectly()
        {
            // Arrange
            var tapData = new TapData
            {
                fingerId = 0,
                position = new Vector2(100, 200),
                startTime = Time.time,
                duration = 0.15f,
                isDoubleTap = false,
                isTwoFinger = false
            };

            // Assert
            Assert.AreEqual(0, tapData.fingerId);
            Assert.AreEqual(new Vector2(100, 200), tapData.position);
            Assert.AreEqual(0.15f, tapData.duration);
            Assert.IsFalse(tapData.isDoubleTap);
            Assert.IsFalse(tapData.isTwoFinger);
        }

        [Test]
        public void Recognizer_TapData_DoubleTap_FlagsCorrectly()
        {
            // Arrange
            var tapData = new TapData
            {
                fingerId = 0,
                position = Vector2.zero,
                startTime = Time.time,
                duration = 0.1f,
                isDoubleTap = true,
                isTwoFinger = false
            };

            // Assert
            Assert.IsTrue(tapData.isDoubleTap);
            Assert.IsFalse(tapData.isTwoFinger);
        }

        [Test]
        public void Recognizer_TapData_TwoFinger_FlagsCorrectly()
        {
            // Arrange
            var tapData = new TapData
            {
                fingerId = 1,
                position = Vector2.zero,
                startTime = Time.time,
                duration = 0.1f,
                isDoubleTap = false,
                isTwoFinger = true
            };

            // Assert
            Assert.IsFalse(tapData.isDoubleTap);
            Assert.IsTrue(tapData.isTwoFinger);
        }
    }
}
