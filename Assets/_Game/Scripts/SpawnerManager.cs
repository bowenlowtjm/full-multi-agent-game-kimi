using UnityEngine;
using System.Collections.Generic;

namespace Arcade.Game
{
    public class SpawnerManager : MonoBehaviour
    {
        public static SpawnerManager Instance { get; private set; }

        [Header("Spawning")]
        [SerializeField] private GameObject targetPrefab;
        [SerializeField] private TargetDefinition[] targetDefinitions;
        [SerializeField] private Transform targetContainer;

        [Header("Config")]
        [SerializeField] private float initialSpawnRate = 1.5f;
        [SerializeField] private float minimumSpawnRate = 0.4f;
        [SerializeField] private float difficultyRampDelay = 10f;
        [SerializeField] private float spawnRateReductionStep = 0.05f;
        [SerializeField] private int maxConcurrentTargets = 4;
        [SerializeField] private float minDistanceBetweenTargets = 80f;

        [Header("Safe Zone")]
        [SerializeField] private Vector2 safeZoneMin = new Vector2(0.05f, 0.10f);
        [SerializeField] private Vector2 safeZoneMax = new Vector2(0.95f, 0.85f);

        [Header("State")]
        [SerializeField] private float currentSpawnRate;
        [SerializeField] private float spawnTimer;
        [SerializeField] private float gameTimer;
        private List<Target> activeTargets = new List<Target>();

        public List<Target> ActiveTargets => activeTargets;

        private Camera mainCamera;
        private bool isSpawning = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            mainCamera = Camera.main;
            currentSpawnRate = initialSpawnRate;
        }

        private void OnEnable()
        {
            if (GameStateManager.Instance != null)
                GameStateManager.Instance.OnStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            if (GameStateManager.Instance != null)
                GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
        }

        private void Update()
        {
            if (!isSpawning) return;

            gameTimer += Time.deltaTime;

            // Difficulty ramping
            if (gameTimer >= difficultyRampDelay)
            {
                currentSpawnRate = Mathf.Max(minimumSpawnRate, currentSpawnRate - spawnRateReductionStep);
                gameTimer = 0f;
            }

            // Spawning
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= currentSpawnRate)
            {
                spawnTimer = 0f;
                TrySpawnTarget();
            }
        }

        private void OnGameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.GAMEPLAY)
            {
                StartSpawning();
            }
            else
            {
                StopSpawning();
            }
        }

        public void StartSpawning()
        {
            isSpawning = true;
            currentSpawnRate = initialSpawnRate;
            gameTimer = 0f;
            spawnTimer = 0f;
        }

        public void StopSpawning()
        {
            isSpawning = false;
        }

        public void ClearAllTargets()
        {
            foreach (var target in activeTargets)
            {
                if (target != null)
                    Destroy(target.gameObject);
            }
            activeTargets.Clear();
        }

        private void TrySpawnTarget()
        {
            if (activeTargets.Count >= maxConcurrentTargets) return;
            if (targetDefinitions == null || targetDefinitions.Length == 0) return;
            if (targetPrefab == null) return;

            // Pick random target type
            TargetDefinition def = targetDefinitions[Random.Range(0, targetDefinitions.Length)];

            // Find valid spawn position
            Vector3? spawnPos = FindValidSpawnPosition();
            if (spawnPos == null) return;

            // Spawn
            GameObject go = Instantiate(targetPrefab, spawnPos.Value, Quaternion.identity, targetContainer);
            Target target = go.GetComponent<Target>();
            if (target != null)
            {
                target.Init(def);
                target.OnGestureAttempt += OnTargetGestureAttempt;
                target.OnTargetExpired += OnTargetExpired;
                activeTargets.Add(target);
            }
        }

        private Vector3? FindValidSpawnPosition()
        {
            // Convert safe zone to screen coordinates
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            Vector2 minScreen = new Vector2(screenWidth * safeZoneMin.x, screenHeight * safeZoneMin.y);
            Vector2 maxScreen = new Vector2(screenWidth * safeZoneMax.x, screenHeight * safeZoneMax.y);

            // Try positions until we find a valid one
            int attempts = 20;
            for (int i = 0; i < attempts; i++)
            {
                float x = Random.Range(minScreen.x, maxScreen.x);
                float y = Random.Range(minScreen.y, maxScreen.y);
                Vector2 screenPos = new Vector2(x, y);

                if (IsPositionValid(screenPos))
                {
                    Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(x, y, mainCamera.nearClipPlane + 10f));
                    worldPos.z = 0f;
                    return worldPos;
                }
            }

            return null;
        }

        private bool IsPositionValid(Vector2 screenPos)
        {
            foreach (var target in activeTargets)
            {
                if (target == null) continue;

                Vector2 targetScreenPos = mainCamera.WorldToScreenPoint(target.transform.position);
                float dist = Vector2.Distance(screenPos, targetScreenPos);

                if (dist < minDistanceBetweenTargets)
                    return false;
            }

            return true;
        }

        private void OnTargetGestureAttempt(Target target, GestureType gesture)
        {
            // Verify gesture matches target requirement
            if (target != null && target.definition != null && target.definition.requiredGesture == gesture)
            {
                // Success
                EffectsManager.Instance?.PlayHitEffect(target.transform.position, target.definition.targetColor, target.definition.baseScore);
                ScoreManager.Instance?.AddScore(target.definition.baseScore);
                ScoreManager.Instance?.RegisterHit();
                target.Expire(true);
                RemoveTarget(target);
                target.DestroyTarget();
            }
            else
            {
                // Wrong gesture - counts as miss
                if (target != null)
                {
                    EffectsManager.Instance?.PlayMissEffect(target.transform.position);
                }
                ScoreManager.Instance?.RegisterMiss();
            }
        }

        private void OnTargetExpired(Target target)
        {
            if (target != null)
            {
                EffectsManager.Instance?.PlayMissEffect(target.transform.position);
            }
            ScoreManager.Instance?.RegisterMiss();
            RemoveTarget(target);
            if (target != null)
                target.DestroyTarget();
        }

        private void RemoveTarget(Target target)
        {
            if (target != null)
            {
                target.OnGestureAttempt -= OnTargetGestureAttempt;
                target.OnTargetExpired -= OnTargetExpired;
            }
            activeTargets.Remove(target);
        }
    }
}
