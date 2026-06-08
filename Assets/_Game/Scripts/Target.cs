using UnityEngine;

namespace Arcade.Game
{
    public class Target : MonoBehaviour
    {
        [Header("Config")]
        public TargetDefinition definition;

        [Header("Refs")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private UnityEngine.UI.Text gestureText;

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

        public TargetType TargetType => definition != null ? definition.targetType : TargetType.Pop;
        public bool IsTrashTarget => TargetType == TargetType.Trash;
        public bool IsHeld => isHeld;
        public float HoldProgress => definition != null ? holdTime / definition.longPressDuration : 0f;
        public float RemainingLifetime => lifetime;

        private void Awake()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            originalScale = transform.localScale;
        }

        public void Init(TargetDefinition def)
        {
            if (def == null)
            {
                Debug.LogError("Target.Init called with null definition!");
                return;
            }

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
            if (isExpired || definition == null) return;

            if (GameStateManager.Instance != null && 
                GameStateManager.Instance.CurrentState != GameState.GAMEPLAY)
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
            if (isExpired || isHeld || definition == null) return;

            fingerId = id;
            isHeld = true;
            holdTime = 0f;

            // Pop visual
            StopAllCoroutines();
            StartCoroutine(PopAnimation());
        }

        private System.Collections.IEnumerator PopAnimation()
        {
            Vector3 target = originalScale * 1.2f;
            float elapsed = 0f;
            while (elapsed < 0.1f)
            {
                transform.localScale = Vector3.Lerp(originalScale, target, elapsed / 0.1f);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localScale = target;
            elapsed = 0f;
            while (elapsed < 0.1f)
            {
                transform.localScale = Vector3.Lerp(target, originalScale, elapsed / 0.1f);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localScale = originalScale;
        }

        public void OnTouchUp(int id)
        {
            if (isExpired || fingerId != id || definition == null) return;

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
                    if (ScoreManager.Instance != null)
                        ScoreManager.Instance.BreakCombo();
                }
            }

            isHeld = false;
            fingerId = -1;
            holdTime = 0f;
        }

        public void OnDoubleTap(int id)
        {
            if (isExpired || definition == null) return;

            if (TargetType == TargetType.Heavy)
            {
                OnGestureAttempt?.Invoke(this, GestureType.DoubleTap);
                Expire(true);
            }
            else
            {
                // Wrong gesture
                OnGestureAttempt?.Invoke(this, GestureType.DoubleTap);
                if (ScoreManager.Instance != null)
                    ScoreManager.Instance.BreakCombo();
            }
        }

        public void OnLongPressComplete(int id)
        {
            if (isExpired || definition == null) return;

            if (TargetType == TargetType.Charge)
            {
                CompleteHold();
            }
        }

        private void CompleteHold()
        {
            isHeld = false;
            fingerId = -1;
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
