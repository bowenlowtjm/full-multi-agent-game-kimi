using UnityEngine;

namespace Arcade.Game
{
    public class TargetDefinitionGenerator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpawnerManager spawnerManager;

        [Header("Sprites")]
        [SerializeField] private Sprite popSprite;
        [SerializeField] private Sprite heavySprite;
        [SerializeField] private Sprite chargeSprite;
        [SerializeField] private Sprite trashSprite;

        void Awake()
        {
            GenerateDefinitions();
        }

        void GenerateDefinitions()
        {
            // TG-01 Pop: Blue Sphere, Single Tap, 10 pts
            TargetDefinition pop = ScriptableObject.CreateInstance<TargetDefinition>();
            pop.targetType = TargetType.Pop;
            pop.displayName = "Pop";
            pop.targetColor = new Color32(33, 150, 243, 255); // Bright Blue
            pop.targetSprite = popSprite;
            pop.requiredGesture = GestureType.SingleTap;
            pop.baseScore = 10;
            pop.lifetime = 5f;

            // TG-02 Heavy: Gold Square, Double Tap, 25 pts
            TargetDefinition heavy = ScriptableObject.CreateInstance<TargetDefinition>();
            heavy.targetType = TargetType.Heavy;
            heavy.displayName = "Heavy";
            heavy.targetColor = new Color32(255, 193, 7, 255); // Gold/Amber
            heavy.targetSprite = heavySprite;
            heavy.requiredGesture = GestureType.DoubleTap;
            heavy.doubleTapInterval = 0.3f;
            heavy.baseScore = 25;
            heavy.lifetime = 4.5f;

            // TG-03 Charge: Green Anchor, Long Press 1.5s, 50 pts
            TargetDefinition charge = ScriptableObject.CreateInstance<TargetDefinition>();
            charge.targetType = TargetType.Charge;
            charge.displayName = "Charge";
            charge.targetColor = new Color32(76, 175, 80, 255); // Deep Green
            charge.targetSprite = chargeSprite;
            charge.requiredGesture = GestureType.LongPress;
            charge.longPressDuration = 1.5f;
            charge.baseScore = 50;
            charge.lifetime = 6f;

            // TG-04 Trash: Red jagged line, Drag to bin, 40 pts
            TargetDefinition trash = ScriptableObject.CreateInstance<TargetDefinition>();
            trash.targetType = TargetType.Trash;
            trash.displayName = "Trash";
            trash.targetColor = new Color32(244, 67, 54, 255); // Alert Red
            trash.targetSprite = trashSprite;
            trash.requiredGesture = GestureType.SwipeTap;
            trash.baseScore = 40;
            trash.lifetime = 8f;

            // Assign to spawner
            if (spawnerManager != null)
            {
                spawnerManager.targetDefinitions = new TargetDefinition[] { pop, heavy, charge, trash };
            }
        }
    }
}
