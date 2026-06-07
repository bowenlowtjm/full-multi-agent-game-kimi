using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Arcade.Game
{
    /// <summary>
    /// Recognizes 5 gestures: SingleTap, DoubleTap, LongPress, SwipeTap, TwoFingerTap
    /// </summary>
    public class GestureRecognizer : MonoBehaviour
    {
        public static GestureRecognizer Instance { get; private set; }

        [Header("Gesture Thresholds")]
        [SerializeField] private float singleTapMaxDuration = 0.2f;
        [SerializeField] private float doubleTapInterval = 0.3f;
        [SerializeField] private float longPressDuration = 0.5f;
        [SerializeField] private float swipeMinDistance = 50f;
        [SerializeField] private float swipeMaxDuration = 0.3f;
        [SerializeField] private float twoFingerMaxDelta = 0.1f;

        [Header("Debug")]
        [SerializeField] private bool logGestures = false;

        // Event: gesture detected at position with direction (for swipe)
        public delegate void GestureHandler(GestureType gesture, Vector2 position, Vector2 direction);
        public event GestureHandler OnGestureDetected;

        // Touch tracking state
        private class TouchState
        {
            public bool isTracking;
            public Vector2 startPos;
            public float startTime;
            public bool longPressTriggered;
            public Coroutine longPressCoroutine;
        }

        private TouchState[] touchStates;
        private float lastSingleTapTime;
        private Vector2 lastSingleTapPos;
        private Coroutine doubleTapCheckCoroutine;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            touchStates = new TouchState[10]; // Support up to 10 touches
            for (int i = 0; i < 10; i++)
                touchStates[i] = new TouchState();
        }

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
            // Cancel all pending coroutines
            for (int i = 0; i < 10; i++)
            {
                if (touchStates[i].longPressCoroutine != null)
                {
                    StopCoroutine(touchStates[i].longPressCoroutine);
                    touchStates[i].longPressCoroutine = null;
                }
            }
            if (doubleTapCheckCoroutine != null)
            {
                StopCoroutine(doubleTapCheckCoroutine);
                doubleTapCheckCoroutine = null;
            }
        }

        private void Update()
        {
            ProcessTouches();
        }

        private void ProcessTouches()
        {
            var activeTouches = Touch.activeTouches;
            int touchCount = activeTouches.Count;

            // Process each touch
            for (int i = 0; i < touchCount; i++)
            {
                var touch = activeTouches[i];
                int idx = touch.touchId;
                if (idx < 0 || idx >= 10) continue;

                var state = touchStates[idx];

                switch (touch.phase)
                {
                    case UnityEngine.InputSystem.TouchPhase.Began:
                        OnTouchBegan(touch, state);
                        break;

                    case UnityEngine.InputSystem.TouchPhase.Moved:
                        OnTouchMoved(touch, state);
                        break;

                    case UnityEngine.InputSystem.TouchPhase.Ended:
                    case UnityEngine.InputSystem.TouchPhase.Canceled:
                        OnTouchEnded(touch, state);
                        break;
                }
            }

            // Check for two-finger tap (simultaneous release)
            if (touchCount >= 2)
            {
                CheckForTwoFingerTap(activeTouches);
            }
        }

        private void OnTouchBegan(Touch touch, TouchState state)
        {
            state.isTracking = true;
            state.startPos = touch.screenPosition;
            state.startTime = Time.time;
            state.longPressTriggered = false;

            // Start long press detection
            if (state.longPressCoroutine != null)
                StopCoroutine(state.longPressCoroutine);
            state.longPressCoroutine = StartCoroutine(LongPressDetection(touch, state));
        }

        private void OnTouchMoved(Touch touch, TouchState state)
        {
            if (!state.isTracking) return;

            // Check if moved too far for tap
            float dist = Vector2.Distance(touch.screenPosition, state.startPos);
            if (dist > swipeMinDistance * 0.5f && !state.longPressTriggered)
            {
                // Cancel long press - this is a potential swipe
            }
        }

        private void OnTouchEnded(Touch touch, TouchState state)
        {
            if (!state.isTracking) return;
            state.isTracking = false;

            // Cancel long press
            if (state.longPressCoroutine != null)
            {
                StopCoroutine(state.longPressCoroutine);
                state.longPressCoroutine = null;
            }

            // If long press already triggered, don't process as tap
            if (state.longPressTriggered)
                return;

            float duration = Time.time - state.startTime;
            float distance = Vector2.Distance(touch.screenPosition, state.startPos);

            // Check for swipe
            if (distance >= swipeMinDistance && duration <= swipeMaxDuration)
            {
                Vector2 direction = (touch.screenPosition - state.startPos).normalized;
                FireGesture(GestureType.SwipeTap, state.startPos, direction);
                return;
            }

            // Check for tap (quick, minimal movement)
            if (duration <= singleTapMaxDuration && distance < swipeMinDistance * 0.5f)
            {
                // Check for double tap
                if (Time.time - lastSingleTapTime <= doubleTapInterval &&
                    Vector2.Distance(state.startPos, lastSingleTapPos) < 50f)
                {
                    // This is a double tap - cancel the delayed single tap
                    if (doubleTapCheckCoroutine != null)
                    {
                        StopCoroutine(doubleTapCheckCoroutine);
                        doubleTapCheckCoroutine = null;
                    }
                    FireGesture(GestureType.DoubleTap, state.startPos, Vector2.zero);
                    lastSingleTapTime = 0; // Reset
                }
                else
                {
                    // Queue single tap check (delayed to allow for double tap)
                    if (doubleTapCheckCoroutine != null)
                        StopCoroutine(doubleTapCheckCoroutine);
                    doubleTapCheckCoroutine = StartCoroutine(DelayedSingleTap(state.startPos));
                    lastSingleTapTime = Time.time;
                    lastSingleTapPos = state.startPos;
                }
            }
        }

        private IEnumerator LongPressDetection(Touch touch, TouchState state)
        {
            yield return new WaitForSeconds(longPressDuration);

            if (state.isTracking && !state.longPressTriggered)
            {
                state.longPressTriggered = true;
                FireGesture(GestureType.LongPress, state.startPos, Vector2.zero);
            }
        }

        private IEnumerator DelayedSingleTap(Vector2 position)
        {
            yield return new WaitForSeconds(doubleTapInterval);
            FireGesture(GestureType.SingleTap, position, Vector2.zero);
        }

        private void CheckForTwoFingerTap(Unity.Collections.ReadOnlyArray<Touch> touches)
        {
            // Two-finger tap = both fingers tap near-simultaneously
            // For simplicity, detect when exactly 2 touches are active and both just began
            if (touches.Count != 2) return;

            var t1 = touches[0];
            var t2 = touches[1];

            // Check if both just started
            if (t1.phase == UnityEngine.InputSystem.TouchPhase.Began &&
                t2.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                // Check they're close enough in time and space
                float timeDelta = Mathf.Abs(t1.startTime - t2.startTime);
                float dist = Vector2.Distance(t1.screenPosition, t2.screenPosition);

                if (timeDelta <= twoFingerMaxDelta && dist < 200f)
                {
                    // Two-finger tap detected
                    Vector2 center = (t1.screenPosition + t2.screenPosition) * 0.5f;
                    FireGesture(GestureType.TwoFingerTap, center, Vector2.zero);
                }
            }
        }

        private void FireGesture(GestureType gesture, Vector2 position, Vector2 direction)
        {
            if (logGestures)
                Debug.Log($"[Gesture] {gesture} at {position}, dir={direction}");

            OnGestureDetected?.Invoke(gesture, position, direction);
        }

        // For mouse testing in editor
        private void OnMouseDown()
        {
            // Handle mouse as single tap for editor testing
            if (!Application.isEditor) return;
            // Raycast handled by InputManager
        }
    }
}
