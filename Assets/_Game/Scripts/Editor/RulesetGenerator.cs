using System.Collections.Generic;
using UnityEngine;

namespace Arcade.Game
{
    /// <summary>
    /// Generates RulesetDefinition ScriptableObject assets in the Editor.
    /// Run via: Assets > Pully > Generate Rulesets
    /// </summary>
    public static class RulesetGenerator
    {
        // Standard ruleset mapping from GAME-SPEC.md
        public static RulesetDefinition GenerateDefaultRuleset()
        {
            var ruleset = ScriptableObject.CreateInstance<RulesetDefinition>();
            ruleset.name = "DefaultRuleset";

            // Rules: Shape + Color -> Gesture + Score
            // Circle (Green) - Single Tap - 100 pts
            // Square (Blue) - Double Tap - 200 pts
            // Triangle (Yellow) - Long Press - 150 pts
            // Star (Purple) - Swipe Tap - 250 pts

            ruleset.rules = new List<RulesetDefinition.TargetRule>
            {
                new RulesetDefinition.TargetRule
                {
                    shape = RulesetDefinition.Shape.Circle,
                    color = new Color(0.298f, 0.686f, 0.314f), // Green #4CAF50
                    requiredGesture = RulesetDefinition.Gesture.SingleTap,
                    baseReward = 100
                },
                new RulesetDefinition.TargetRule
                {
                    shape = RulesetDefinition.Shape.Square,
                    color = new Color(0.129f, 0.588f, 0.953f), // Blue #2196F3
                    requiredGesture = RulesetDefinition.Gesture.DoubleTap,
                    baseReward = 200
                },
                new RulesetDefinition.TargetRule
                {
                    shape = RulesetDefinition.Shape.Triangle,
                    color = new Color(1f, 0.922f, 0.231f), // Yellow #FFEB3B
                    requiredGesture = RulesetDefinition.Gesture.LongPress,
                    baseReward = 150
                },
                new RulesetDefinition.TargetRule
                {
                    shape = RulesetDefinition.Shape.Star,
                    color = new Color(0.612f, 0.153f, 0.698f), // Purple #9C27B0
                    requiredGesture = RulesetDefinition.Gesture.SwipeTap,
                    baseReward = 250
                }
            };

            // Game settings
            ruleset.comboStep = 1.1f;
            ruleset.comboCap = 5f;
            ruleset.lives = 3;
            ruleset.roundSeconds = 60f;
            ruleset.targetLifetime = 4f;
            ruleset.spawnIntervalStart = 1.2f;
            ruleset.spawnIntervalEnd = 0.6f;
            ruleset.maxConcurrentTargets = 4;

            // Gesture thresholds
            ruleset.doubleTapWindow = 0.3f;
            ruleset.longPressDuration = 0.5f;
            ruleset.swipeMinDistance = 50f;

            // Determinism seed
            ruleset.seed = 12345;

            return ruleset;
        }
    }
}
