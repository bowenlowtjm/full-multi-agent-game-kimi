using UnityEngine;

namespace Arcade.Game
{
    public enum TargetType
    {
        Pop,      // TG-01: Blue Sphere, Single Tap
        Heavy,    // TG-02: Gold Square, Double Tap
        Charge,   // TG-03: Green Anchor, Long Press 1.5s
        Trash     // TG-04: Red jagged line, Drag
    }

    public enum GestureType
    {
        None,
        SingleTap,
        DoubleTap,
        LongPress,
        Drag
    }

    [CreateAssetMenu(fileName = "NewTarget", menuName = "Arcade/Target Definition")]
    public class TargetDefinition : ScriptableObject
    {
        [Header("Identity")]
        public TargetType targetType;
        public string displayName;

        [Header("Visual")]
        public Color targetColor = Color.white;
        public Sprite targetSprite;

        [Header("Gesture")]
        public GestureType requiredGesture;
        public float longPressDuration = 1.5f;
        public float doubleTapInterval = 0.3f;

        [Header("Scoring")]
        public int baseScore = 10;
        public float lifetime = 5f;

        [Header("Audio")]
        public AudioClip successSFX;
        public AudioClip failSFX;

        public string GetGestureDescription()
        {
            switch (requiredGesture)
            {
                case GestureType.SingleTap: return "TAP";
                case GestureType.DoubleTap: return "DOUBLE TAP";
                case GestureType.LongPress: return $"HOLD {longPressDuration}s";
                case GestureType.Drag: return "DRAG TO BIN";
                default: return "";
            }
        }
    }
}
