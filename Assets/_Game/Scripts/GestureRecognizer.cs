using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Arcade.Game
{
    public class GestureRecognizer : MonoBehaviour
    {
        public static GestureRecognizer Instance { get; private set; }

        [Header("Config")]
        [SerializeField] private float doubleTapInterval = 0.3f;
        [SerializeField] private float longPressDuration = 1.5f;
        [SerializeField] private float dragThreshold = 10f;

        [Header("State")]
        private bool isTouching = false;
        private bool isDragging = false;
        private float touchStartTime;
        private Vector2 touchStartPos;
        private int tapCount = 0;
        private float lastTapTime;
        private Touch activeTouch;

        public delegate void GestureHandler(GestureType gesture, Vector2 position, Vector2 delta);
        public event GestureHandler OnGestureDetected;
        public event System.Action<Vector2> OnDragStart;
        public event System.Action<Vector2, Vector2> OnDragUpdate;
        public event System.Action<Vector2, Vector2> OnDragEnd;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }

        private void Update()
        {
            ProcessTouchInput();
        }

        private void ProcessTouchInput()
        {
            if (Touch.activeTouches.Count == 0)
            {
                if (isTouching && !isDragging)
                {
                    // Touch ended - check for tap
                    float touchDuration = Time.time - touchStartTime;
                    if (touchDuration < longPressDuration)
                    {
                        if (tapCount == 0)
                        {
                            tapCount = 1;
                            lastTapTime = Time.time;
                        }
                        else if (Time.time - lastTapTime <= doubleTapInterval)
                        {
                            OnGestureDetected?.Invoke(GestureType.DoubleTap, touchStartPos, Vector2.zero);
                            tapCount = 0;
                        }
                    }
                }

                if (isDragging)
                {
                    OnDragEnd?.Invoke(activeTouch.screenPosition, activeTouch.delta);
                    isDragging = false;
                }

                isTouching = false;
                return;
            }

            var touch = Touch.activeTouches[0];

            switch (touch.phase)
            {
                case UnityEngine.InputSystem.TouchPhase.Began:
                    touchStartTime = Time.time;
                    touchStartPos = touch.screenPosition;
                    isTouching = true;
                    isDragging = false;
                    activeTouch = touch;

                    // Check for double tap
                    if (tapCount > 0 && Time.time - lastTapTime <= doubleTapInterval)
                    {
                        tapCount = 0;
                        OnGestureDetected?.Invoke(GestureType.DoubleTap, touchStartPos, Vector2.zero);
                        return;
                    }
                    tapCount = 0;
                    break;

                case UnityEngine.InputSystem.TouchPhase.Moved:
                    if (isTouching && !isDragging)
                    {
                        float dist = Vector2.Distance(touch.screenPosition, touchStartPos);
                        if (dist > dragThreshold)
                        {
                            isDragging = true;
                            OnDragStart?.Invoke(touchStartPos);
                        }
                    }

                    if (isDragging)
                    {
                        OnDragUpdate?.Invoke(touch.screenPosition, touch.delta);
                    }
                    break;

                case UnityEngine.InputSystem.TouchPhase.Ended:
                    if (isDragging)
                    {
                        OnDragEnd?.Invoke(touch.screenPosition, touch.delta);
                        isDragging = false;
                    }
                    else if (isTouching)
                    {
                        float touchDuration = Time.time - touchStartTime;
                        if (touchDuration >= longPressDuration)
                        {
                            OnGestureDetected?.Invoke(GestureType.LongPress, touchStartPos, Vector2.zero);
                            tapCount = 0;
                        }
                        else
                        {
                            // Single tap - delay to check for double tap
                            if (tapCount == 0)
                            {
                                tapCount = 1;
                                lastTapTime = Time.time;
                                // Fire single tap immediately (if no second tap comes)
                                StartCoroutine(DelayedSingleTap(touchStartPos));
                            }
                        }
                    }
                    isTouching = false;
                    activeTouch = default;
                    break;
            }
        }

        private System.Collections.IEnumerator DelayedSingleTap(Vector2 pos)
        {
            yield return new WaitForSeconds(doubleTapInterval);
            if (tapCount == 1)
            {
                OnGestureDetected?.Invoke(GestureType.SingleTap, pos, Vector2.zero);
                tapCount = 0;
            }
        }

        public bool IsCurrentlyDragging => isDragging;
        public Vector2 CurrentTouchPos => isTouching ? activeTouch.screenPosition : Vector2.zero;
    }
}
