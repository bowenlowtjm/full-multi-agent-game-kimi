using UnityEngine;
using UnityEngine.UI;

namespace Arcade.Game
{
    /// <summary>
    /// M4-006 Juice & Polish: Button press feedback with scale animation
    /// Attach to any Button GameObject for automatic press effect
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ButtonEffects : MonoBehaviour
    {
        [Header("Press Effect")]
        [SerializeField] private bool useEffectsManager = true;
        [SerializeField] private float customPressScale = 0.9f;
        [SerializeField] private float customPressDuration = 0.1f;

        private Button button;
        private Transform buttonTransform;
        private Vector3 originalScale;

        private void Awake()
        {
            button = GetComponent<Button>();
            buttonTransform = transform;
            originalScale = buttonTransform.localScale;

            // Hook into button events
            button.onClick.AddListener(OnButtonPressed);
        }

        private void OnDestroy()
        {
            if (button != null)
                button.onClick.RemoveListener(OnButtonPressed);
        }

        private void OnButtonPressed()
        {
            // Play button audio
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayButton();

            if (useEffectsManager && EffectsManager.Instance != null)
            {
                EffectsManager.Instance.ButtonPressEffect(buttonTransform);
            }
            else
            {
                StartCoroutine(AnimatePress());
            }
        }

        private System.Collections.IEnumerator AnimatePress()
        {
            Vector3 pressedScale = originalScale * customPressScale;

            // Scale down
            float timer = 0f;
            float duration = customPressDuration * 0.5f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                buttonTransform.localScale = Vector3.Lerp(originalScale, pressedScale, t);
                yield return null;
            }

            // Scale back with slight bounce
            timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                
                // Slight overshoot
                if (t < 0.5f)
                {
                    float tt = t * 2f;
                    buttonTransform.localScale = Vector3.Lerp(pressedScale, originalScale * 1.05f, tt);
                }
                else
                {
                    float tt = (t - 0.5f) * 2f;
                    buttonTransform.localScale = Vector3.Lerp(originalScale * 1.05f, originalScale, tt);
                }
                yield return null;
            }

            buttonTransform.localScale = originalScale;
        }
    }
}
