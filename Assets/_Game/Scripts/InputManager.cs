using UnityEngine;
using System.Collections.Generic;

namespace Arcade.Game
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        [Header("Config")]
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float dragThreshold = 10f;
        [SerializeField] private float doubleTapInterval = 0.3f;

        [Header("State")]
        private Dictionary<int, TouchData> activeTouches = new Dictionary<int, TouchData>();
        private Dictionary<int, float> lastTapTimes = new Dictionary<int, float>();
        private Dictionary<int, Vector2> lastTapPositions = new Dictionary<int, Vector2>();
        private Camera mainCamera;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            mainCamera = Camera.main;
        }

        private void Update()
        {
            // Touch input
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                ProcessTouch(touch);
            }

            // Mouse input for editor testing
            if (Input.touchCount == 0)
            {
                ProcessMouse();
            }
        }

        private void ProcessTouch(Touch touch)
        {
            Vector2 screenPos = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchBegan(touch.fingerId, screenPos);
                    break;

                case TouchPhase.Moved:
                    OnTouchMoved(touch.fingerId, screenPos);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    OnTouchEnded(touch.fingerId, screenPos);
                    break;
            }
        }

        private void ProcessMouse()
        {
            Vector2 screenPos = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                OnTouchBegan(0, screenPos);
            }
            else if (Input.GetMouseButton(0))
            {
                OnTouchMoved(0, screenPos);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnTouchEnded(0, screenPos);
            }
        }

        private void OnTouchBegan(int fingerId, Vector2 screenPos)
        {
            Ray ray = mainCamera.ScreenPointToRay(screenPos);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, targetLayer);

            Target hitTarget = hit.collider?.GetComponent<Target>();

            // Check for double tap
            bool isDoubleTap = false;
            if (lastTapTimes.TryGetValue(fingerId, out float lastTime))
            {
                if (Time.time - lastTime <= doubleTapInterval)
                {
                    if (lastTapPositions.TryGetValue(fingerId, out Vector2 lastPos))
                    {
                        float dist = Vector2.Distance(screenPos, lastPos);
                        if (dist < dragThreshold)
                        {
                            isDoubleTap = true;
                        }
                    }
                }
            }

            activeTouches[fingerId] = new TouchData
            {
                fingerId = fingerId,
                startPos = screenPos,
                currentPos = screenPos,
                startTime = Time.time,
                targetHit = hitTarget,
                isDragging = false,
                isDoubleTap = isDoubleTap
            };

            if (hitTarget != null)
            {
                if (isDoubleTap)
                {
                    hitTarget.OnDoubleTap(fingerId);
                }
                else
                {
                    hitTarget.OnTouchDown(fingerId, screenPos);
                }
            }

            // For touch: record tap immediately
            // For mouse double-click: need to wait
            if (!isDoubleTap)
            {
                lastTapTimes[fingerId] = Time.time;
                lastTapPositions[fingerId] = screenPos;
            }
            else
            {
                lastTapTimes.Remove(fingerId);
                lastTapPositions.Remove(fingerId);
            }
        }

        private void OnTouchMoved(int fingerId, Vector2 screenPos)
        {
            if (!activeTouches.TryGetValue(fingerId, out TouchData touch)) return;

            touch.currentPos = screenPos;

            // Check for drag start (only for trash)
            if (!touch.isDragging && touch.targetHit != null && touch.targetHit.IsTrashTarget)
            {
                float dist = Vector2.Distance(touch.startPos, screenPos);
                if (dist > dragThreshold)
                {
                    touch.isDragging = true;
                }
            }

            if (touch.isDragging && touch.targetHit != null)
            {
                // Move target with finger
                Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
                worldPos.z = touch.targetHit.transform.position.z;
                touch.targetHit.transform.position = Vector3.Lerp(
                    touch.targetHit.transform.position, worldPos, 0.3f);
            }

            activeTouches[fingerId] = touch;
        }

        private void OnTouchEnded(int fingerId, Vector2 screenPos)
        {
            if (!activeTouches.TryGetValue(fingerId, out TouchData touch)) return;

            float duration = Time.time - touch.startTime;

            if (touch.targetHit != null)
            {
                if (touch.isDragging && touch.targetHit.IsTrashTarget)
                {
                    // Drag ended - check if in trash zone
                    CheckTrashDrop(touch.targetHit, screenPos);
                }
                else if (!touch.isDoubleTap)
                {
                    // Check for long press or single tap
                    if (duration >= touch.targetHit.definition.longPressDuration &&
                        touch.targetHit.TargetType == TargetType.Charge)
                    {
                        touch.targetHit.OnLongPressComplete(fingerId);
                    }
                    else
                    {
                        touch.targetHit.OnTouchUp(fingerId);
                    }
                }
            }

            activeTouches.Remove(fingerId);
        }

        private void CheckTrashDrop(Target target, Vector2 screenPos)
        {
            if (target == null) return;

            bool inTrashZone = TrashBinZone.Instance?.IsPointInZone(screenPos) ?? false;

            if (inTrashZone && target.IsTrashTarget)
            {
                // Success!
                EffectsManager.Instance?.PlayHitEffect(target.transform.position, Color.red, target.definition.baseScore);
                target.Expire(true);
                ScoreManager.Instance?.AddScore(target.definition.baseScore);
                ScoreManager.Instance?.RegisterHit();
                TrashBinZone.Instance?.OnTrashReceived();
                target.DestroyTarget();
                SpawnerManager.Instance?.ActiveTargets.Remove(target);
            }
            else
            {
                // Fail - dropped outside bin
                EffectsManager.Instance?.PlayMissEffect(target.transform.position);
                ScoreManager.Instance?.RegisterMiss();
                target.DestroyTarget();
                SpawnerManager.Instance?.ActiveTargets.Remove(target);
            }
        }

        private struct TouchData
        {
            public int fingerId;
            public Vector2 startPos;
            public Vector2 currentPos;
            public float startTime;
            public Target targetHit;
            public bool isDragging;
            public bool isDoubleTap;
        }
    }
}
