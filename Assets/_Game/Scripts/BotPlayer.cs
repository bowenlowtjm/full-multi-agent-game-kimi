using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Arcade.Game
{
    /// <summary>
    /// Automated bot player for testing game balance and detecting softlocks.
    /// Simulates perfect or configurable-accuracy gameplay.
    /// </summary>
    public class BotPlayer : MonoBehaviour
    {
        public static BotPlayer Instance { get; private set; }

        [Header("Config")]
        [SerializeField] private float reactionTime = 0.1f;
        [SerializeField] [Range(0f, 1f)] private float accuracy = 1.0f;
        [SerializeField] private bool logEnabled = true;
        [SerializeField] private bool autoStart = false;

        [Header("State")]
        [SerializeField] private bool isRunning = false;
        [SerializeField] private float sessionTimer = 0f;
        [SerializeField] private int hits = 0;
        [SerializeField] private int misses = 0;
        [SerializeField] private List<float> reactionTimes = new List<float>();

        public bool IsRunning => isRunning;
        public float SessionTime => sessionTimer;
        public int Hits => hits;
        public int Misses => misses;
        public float HitRate => (hits + misses) > 0 ? (float)hits / (hits + misses) : 0f;
        public float AvgReactionTime => reactionTimes.Count > 0 ? reactionTimes.Average() : 0f;

        private float targetAcquireTime = 0f;
        private Target currentTarget = null;
        private Camera mainCamera;

        public delegate void BotSessionHandler(BotSessionResult result);
        public event BotSessionHandler OnSessionComplete;

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

        private void Start()
        {
            if (autoStart)
                StartSession();
        }

        private void Update()
        {
            if (!isRunning) return;

            sessionTimer += Time.deltaTime;

            // Find best target to hit
            Target bestTarget = FindBestTarget();
            if (bestTarget != null && bestTarget != currentTarget)
            {
                currentTarget = bestTarget;
                targetAcquireTime = Time.time;
            }

            // React to target
            if (currentTarget != null)
            {
                float timeSinceAcquire = Time.time - targetAcquireTime;
                if (timeSinceAcquire >= reactionTime)
                {
                    PerformGesture(currentTarget);
                    currentTarget = null;
                }
            }

            // Track hits/misses via events
        }

        public void StartSession()
        {
            isRunning = true;
            sessionTimer = 0f;
            hits = 0;
            misses = 0;
            reactionTimes.Clear();

            // Subscribe to score events
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnComboChanged += OnComboChanged;
            }

            Log("Bot session started");
        }

        public void StopSession()
        {
            isRunning = false;

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnComboChanged -= OnComboChanged;
            }

            var result = new BotSessionResult
            {
                duration = sessionTimer,
                hits = hits,
                misses = misses,
                finalScore = ScoreManager.Instance?.CurrentScore ?? 0,
                avgReactionTime = AvgReactionTime
            };

            Log($"Bot session complete: Score={result.finalScore}, HitRate={result.HitRate:P1}, AvgReaction={result.avgReactionTime:F3}s");
            OnSessionComplete?.Invoke(result);
        }

        private Target FindBestTarget()
        {
            if (SpawnerManager.Instance == null) return null;

            var targets = SpawnerManager.Instance.ActiveTargets;
            if (targets == null || targets.Count == 0) return null;

            // Find target with shortest remaining lifetime
            Target best = null;
            float minTime = float.MaxValue;

            foreach (var target in targets)
            {
                if (target == null) continue;
                float remaining = target.RemainingLifetime;
                if (remaining < minTime)
                {
                    minTime = remaining;
                    best = target;
                }
            }

            return best;
        }

        private void PerformGesture(Target target)
        {
            if (target == null || target.definition == null) return;

            // Check accuracy - sometimes miss
            if (Random.value > accuracy)
            {
                PerformWrongGesture(target);
                return;
            }

            // Record reaction time
            reactionTimes.Add(Time.time - targetAcquireTime);

            // Simulate correct gesture
            GestureType required = target.definition.requiredGesture;
            SimulateGesture(target, required);

            Log($"Bot hit {target.definition.targetType} with {required}");
            hits++;
        }

        private void PerformWrongGesture(Target target)
        {
            // Pick a random wrong gesture
            GestureType wrong = GetRandomWrongGesture(target.definition.requiredGesture);
            SimulateGesture(target, wrong);

            Log($"Bot MISS {target.definition.targetType} (wrong gesture)");
            misses++;
        }

        private void SimulateGesture(Target target, GestureType gesture)
        {
            // Trigger target's gesture handler directly
            switch (gesture)
            {
                case GestureType.SingleTap:
                    target.OnTouchUp(0);
                    break;
                case GestureType.DoubleTap:
                    target.OnDoubleTap(0);
                    break;
                case GestureType.LongPress:
                    target.OnLongPressComplete(0);
                    break;
                case GestureType.SwipeTap:
                    // For trash, simulate drag to bin
                    if (target.IsTrashTarget && TrashBinZone.Instance != null)
                    {
                        target.transform.position = TrashBinZone.Instance.transform.position;
                        CheckTrashDrop(target);
                    }
                    else
                    {
                        target.OnTouchUp(0);
                    }
                    break;
                case GestureType.TwoFingerTap:
                    target.OnTouchUp(0);
                    break;
            }
        }

        private void CheckTrashDrop(Target target)
        {
            if (EffectsManager.Instance != null)
            {
                Color c = target.definition != null ? target.definition.targetColor : Color.red;
                int pts = target.definition != null ? target.definition.baseScore : 40;
                EffectsManager.Instance.PlayHitEffect(target.transform.position, c, pts);
            }
            target.Expire(true);
            if (ScoreManager.Instance != null)
            {
                int pts = target.definition != null ? target.definition.baseScore : 40;
                ScoreManager.Instance.AddScore(pts);
                ScoreManager.Instance.RegisterHit();
            }
            if (SpawnerManager.Instance != null && SpawnerManager.Instance.ActiveTargets.Contains(target))
            {
                SpawnerManager.Instance.ActiveTargets.Remove(target);
            }
            target.DestroyTarget();
        }

        private GestureType GetRandomWrongGesture(GestureType correct)
        {
            GestureType[] all = (GestureType[])System.Enum.GetValues(typeof(GestureType));
            GestureType wrong;
            do
            {
                wrong = all[Random.Range(0, all.Length)];
            } while (wrong == correct);
            return wrong;
        }

        private void OnComboChanged(int multiplier, int hits)
        {
            // Track hits/misses implicitly through combo changes
        }

        private void Log(string message)
        {
            if (!logEnabled) return;
            Debug.Log($"[Bot] {message}");
        }

        private void OnDestroy()
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnComboChanged -= OnComboChanged;
            }
        }
    }

    public struct BotSessionResult
    {
        public float duration;
        public int hits;
        public int misses;
        public int finalScore;
        public float avgReactionTime;

        public float HitRate => (hits + misses) > 0 ? (float)hits / (hits + misses) : 0f;
    }
}
