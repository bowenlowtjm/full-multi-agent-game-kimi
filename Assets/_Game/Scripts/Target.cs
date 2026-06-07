using UnityEngine;

namespace Arcade.Game
{
    public class Target : MonoBehaviour
    {
        [Header("Config")]
        public TargetDefinition definition;

        [Header("Refs")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMesh gestureText;

        [Header("State")]
        [SerializeField] private float lifetime;
        [SerializeField] private bool isHeld = false;
        [SerializeField] private float holdTime = 0f;
        [SerializeField] private int fingerId = -1;

        public System.Action<Target, GestureType> OnGestureAttempt;
        public System.Action<Target> OnTargetExpired;

        private Vector3 originalScale;
        private Color originalColor;
        private bool isExpired = false;

        public TargetType TargetType => definition?.targetType ?? TargetType.Pop;
        public bool IsTrashTarget => TargetType == TargetType.Trash;
        public bool IsHeld => isHeld;
        public float HoldProgress => holdTime / (definition?.longPressDuration ?? 1.5f);

        private void Awake()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            originalScale = transform.localScale;
        }

        public void Init(TargetDefinition def)
        {
            definition = def;
            lifetime = def.lifetime;

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = def.targetSprite;
                spriteRenderer.color = def.targetColor;
                originalColor = def.targetColor;
            }

            if (gestureText != null)
            {
                gestureText.text = def.GetGestureDescription();
            }

            isExpired = false;
            isHeld = false;
            holdTime = 0f;
            fingerId = -1;
        }

        private void Update()
        {
            if (isExpired || GameStateManager.Instance?.CurrentState != GameState.GAMEPLAY)
                return;

            lifetime -= Time.deltaTime;

            // Visual countdown - shrink as lifetime decreases
            float lifeRatio = lifetime / definition.lifetime;
            transform.localScale = originalScale * Mathf.Lerp(0.5f, 1f, lifeRatio);

            // Flash when low on time
            if (lifetime < 1f && spriteRenderer != null)
            {
                float flash = Mathf.PingPong(Time.time * 4f, 1f);
                spriteRenderer.color = Color.Lerp(Color.red, originalColor, flash);
            }

            if (lifetime <= 0f)
            {
                Expire();
            }

            // Track hold time
            if (isHeld)
            {
                holdTime += Time.deltaTime;
                if (TargetType == TargetType.Charge && holdTime >= definition.longPressDuration)
                {
                    // Auto-complete long press
                    CompleteHold();
                }
            }
        }

        public void OnTouchDown(int id, Vector2 screenPos)
        {
            if (isExpired || isHeld) return;

            fingerId = id;
            isHeld = true;
            holdTime = 0f;
        }

        public void OnTouchUp(int id)
        {
            if (isExpired || fingerId != id) return;

            if (TargetType == TargetType.Pop)
            {
                if (holdTime < definition.longPressDuration)
                {
                    OnGestureAttempt?.Invoke(this, GestureType.SingleTap);
                    Expire(true);
                }
            }
            else if (TargetType == TargetType.Heavy)
            {
                // Handled by double-tap detection
            }
            else if (TargetType == TargetType.Charge)
            {
                if (holdTime >= definition.longPressDuration)
                {
                    // Already handled in CompleteHold
                }
                else
                {
                    // Released too early - wrong gesture
                    OnGestureAttempt?.Invoke(this, GestureType.SingleTap);
                    ScoreManager.Instance?.BreakCombo();
                }
            }

            isHeld = false;
            fingerId = -1;
            holdTime = 0f;
        }

        public void OnDoubleTap(int id)
        {
            if (isExpired) return;

            if (TargetType == TargetType.Heavy)
            {
                OnGestureAttempt?.Invoke(this, GestureType.DoubleTap);
                Expire(true);
            }
            else
            {
                // Wrong gesture
                OnGestureAttempt?.Invoke(this, GestureType.DoubleTap);
                ScoreManager.Instance?.BreakCombo();
            }
        }

        public void OnLongPressComplete(int id)
        {
            if (isExpired) return;

            if (TargetType == TargetType.Charge)
            {
                CompleteHold();
            }
        }

        private void CompleteHold()
        {
            OnGestureAttempt?.Invoke(this, GestureType.LongPress);
            Expire(true);
        }

        public void Expire(bool success = false)
        {
            if (isExpired) return;
            isExpired = true;

            if (!success)
            {
                OnTargetExpired?.Invoke(this);
            }
        }

        public void DestroyTarget()
        {
            Destroy(gameObject);
        }
    }
}
