using UnityEngine;

namespace Arcade.Game
{
    public class TrashBinZone : MonoBehaviour
    {
        public static TrashBinZone Instance { get; private set; }

        [Header("Config")]
        [SerializeField] private RectTransform zoneTransform;
        [SerializeField] private float scaleOnTrash = 1.2f;
        [SerializeField] private float scaleDuration = 0.2f;

        [Header("Animation")]
        private Vector3 originalScale;
        private float scaleTimer = 0f;

        public event System.Action OnTrashReceivedEvent;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            if (zoneTransform == null)
                zoneTransform = GetComponent<RectTransform>();

            originalScale = zoneTransform?.localScale ?? Vector3.one;
        }

        private void Update()
        {
            // Animate scale back to normal
            if (scaleTimer > 0f)
            {
                scaleTimer -= Time.deltaTime;
                float t = scaleTimer / scaleDuration;
                if (zoneTransform != null)
                {
                    zoneTransform.localScale = Vector3.Lerp(
                        originalScale * scaleOnTrash,
                        originalScale,
                        1f - t);
                }
            }
        }

        public bool IsPointInZone(Vector2 screenPos)
        {
            if (zoneTransform == null) return false;

            // Convert screen pos to local position in zone
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                zoneTransform, screenPos, Camera.main, out localPoint);

            Rect rect = zoneTransform.rect;
            return rect.Contains(localPoint);
        }

        public void OnTrashReceived()
        {
            scaleTimer = scaleDuration;
            OnTrashReceivedEvent?.Invoke();

            // Visual feedback
            if (zoneTransform != null)
            {
                zoneTransform.localScale = originalScale * scaleOnTrash;
            }
        }
    }
}
