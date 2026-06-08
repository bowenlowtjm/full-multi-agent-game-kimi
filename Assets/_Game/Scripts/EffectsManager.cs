using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Arcade.Game
{
    public class EffectsManager : MonoBehaviour
    {
        public static EffectsManager Instance { get; private set; }

        [Header("Particles")]
        [SerializeField] private ParticleSystem hitParticles;
        [SerializeField] private ParticleSystem missParticles;
        [SerializeField] private ParticleSystem comboParticles;
        [SerializeField] private ParticleSystem highScoreParticles;
        [SerializeField] private Transform particleContainer;

        [Header("Screenshake")]
        [SerializeField] private float shakeDuration = 0.2f;
        [SerializeField] private float shakeMagnitude = 0.3f;
        [SerializeField] private float comboShakeMultiplier = 1.5f;
        private float shakeTimer = 0f;
        private Vector3 originalCamPos;
        private Transform cameraTransform;

        [Header("Flash")]
        [SerializeField] private GameObject flashOverlay;
        [SerializeField] private float flashDuration = 0.1f;
        [SerializeField] private AnimationCurve flashCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

        [Header("Floating Text")]
        [SerializeField] private GameObject scorePopupPrefab;
        [SerializeField] private Transform popupContainer;
        [SerializeField] private float popupFloatSpeed = 2f;
        [SerializeField] private float popupDuration = 1f;

        [Header("Scale Pop")]
        [SerializeField] private float scalePopDuration = 0.15f;
        [SerializeField] private float scalePopAmount = 1.3f;
        [SerializeField] private AnimationCurve scalePopCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Queue<ParticleSystem> hitParticlePool = new Queue<ParticleSystem>();
        private Queue<ParticleSystem> missParticlePool = new Queue<ParticleSystem>();
        private Queue<GameObject> popupPool = new Queue<GameObject>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            cameraTransform = Camera.main?.transform;
            if (cameraTransform != null)
                originalCamPos = cameraTransform.localPosition;
        }

        private void Update()
        {
            // Screenshake
            if (shakeTimer > 0f && cameraTransform != null)
            {
                shakeTimer -= Time.deltaTime;
                float strength = shakeTimer / shakeDuration;
                Vector3 shake = Random.insideUnitSphere * shakeMagnitude * strength;
                shake.z = 0f;
                cameraTransform.localPosition = originalCamPos + shake;
            }
            else if (cameraTransform != null)
            {
                cameraTransform.localPosition = originalCamPos;
            }
        }

        /// <summary>
        /// Play full hit effect: particles, floating text, scale pop, haptic.
        /// </summary>
        public void PlayHitEffect(Vector3 position, Color color, int score, int combo = 1)
        {
            // Particles
            ParticleSystem particles = GetPooledParticle(hitParticlePool, hitParticles, position);
            if (particles != null)
            {
                var main = particles.main;
                main.startColor = color;
                particles.Play();
            }

            // Floating score text
            SpawnFloatingText(position, $"+{score}", color, combo);

            // Haptic feedback
            HapticsManager.Instance?.PlayHitHaptic();

            // Combo escalation
            if (combo >= 3)
            {
                PlayComboEffect(combo);
            }
        }

        /// <summary>
        /// Play miss effect: particles, screenshake, flash, haptic.
        /// </summary>
        public void PlayMissEffect(Vector3 position)
        {
            // Particles
            ParticleSystem particles = GetPooledParticle(missParticlePool, missParticles, position);
            if (particles != null)
            {
                particles.Play();
            }

            // Screenshake (stronger for miss)
            ShakeScreen(1.5f);

            // Flash red
            FlashScreen(Color.red * 0.5f);

            // Haptic feedback
            HapticsManager.Instance?.PlayMissHaptic();
        }

        /// <summary>
        /// Play combo milestone effect with escalating intensity.
        /// </summary>
        public void PlayComboEffect(int combo)
        {
            if (comboParticles != null)
            {
                // Scale particles based on combo
                var main = comboParticles.main;
                main.startSize = 0.5f + (combo * 0.1f);
                comboParticles.Play();
            }

            // Haptic
            HapticsManager.Instance?.PlayComboHaptic(combo);

            // Stronger shake for higher combos
            if (combo >= 5)
            {
                ShakeScreen(comboShakeMultiplier);
            }
        }

        /// <summary>
        /// Play high score celebration effect.
        /// </summary>
        public void PlayHighScoreEffect(Vector3 position)
        {
            if (highScoreParticles != null)
            {
                highScoreParticles.Play();
            }

            // Extended celebration
            ShakeScreen(2f);
            FlashScreen(Color.yellow * 0.3f);
            SpawnFloatingText(position, "NEW HIGH SCORE!", Color.yellow, 5);
        }

        /// <summary>
        /// Trigger screenshake with optional intensity multiplier.
        /// </summary>
        public void ShakeScreen(float intensityMultiplier = 1f)
        {
            shakeTimer = shakeDuration * intensityMultiplier;
        }

        /// <summary>
        /// Flash screen with color.
        /// </summary>
        public void FlashScreen(Color color)
        {
            if (flashOverlay != null)
            {
                StartCoroutine(FlashCoroutine(color));
            }
        }

        /// <summary>
        /// Animate scale pop on a transform.
        /// </summary>
        public void PlayScalePop(Transform target)
        {
            if (target != null)
            {
                StartCoroutine(ScalePopCoroutine(target));
            }
        }

        private System.Collections.IEnumerator FlashCoroutine(Color color)
        {
            SpriteRenderer sr = flashOverlay.GetComponent<SpriteRenderer>();
            if (sr == null) yield break;

            flashOverlay.SetActive(true);
            float timer = 0f;

            while (timer < flashDuration)
            {
                timer += Time.deltaTime;
                float alpha = flashCurve.Evaluate(timer / flashDuration);
                sr.color = new Color(color.r, color.g, color.b, color.a * alpha);
                yield return null;
            }

            flashOverlay.SetActive(false);
        }

        private System.Collections.IEnumerator ScalePopCoroutine(Transform target)
        {
            Vector3 originalScale = target.localScale;
            float timer = 0f;

            while (timer < scalePopDuration)
            {
                timer += Time.deltaTime;
                float t = timer / scalePopDuration;
                float scale = 1f + (scalePopCurve.Evaluate(t) * (scalePopAmount - 1f));
                target.localScale = originalScale * scale;
                yield return null;
            }

            target.localScale = originalScale;
        }

        private void SpawnFloatingText(Vector3 position, string text, Color color, int combo)
        {
            if (scorePopupPrefab == null) return;

            GameObject popup = GetPooledPopup();
            popup.transform.position = position;

            Text textComponent = popup.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.text = text;
                textComponent.color = color;

                // Larger text for combos
                textComponent.fontSize = 24 + (combo * 2);
            }

            StartCoroutine(AnimatePopup(popup, combo));
        }

        private GameObject GetPooledPopup()
        {
            if (popupPool.Count > 0)
            {
                GameObject popup = popupPool.Dequeue();
                popup.SetActive(true);
                return popup;
            }
            return Instantiate(scorePopupPrefab, popupContainer);
        }

        private System.Collections.IEnumerator AnimatePopup(GameObject popup, int combo)
        {
            Vector3 startPos = popup.transform.position;
            float timer = 0f;

            // Faster rise for higher combos
            float speed = popupFloatSpeed * (1f + combo * 0.1f);

            while (timer < popupDuration)
            {
                timer += Time.deltaTime;
                float t = timer / popupDuration;

                // Rise up
                popup.transform.position = startPos + Vector3.up * speed * t;

                // Fade out
                CanvasGroup cg = popup.GetComponent<CanvasGroup>();
                if (cg != null) cg.alpha = 1f - t;

                yield return null;
            }

            // Return to pool
            popup.SetActive(false);
            popup.transform.position = startPos;
            CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
            if (canvasGroup != null) canvasGroup.alpha = 1f;
            popupPool.Enqueue(popup);
        }

        private ParticleSystem GetPooledParticle(Queue<ParticleSystem> pool, ParticleSystem prefab, Vector3 position)
        {
            if (prefab == null) return null;

            ParticleSystem particles;
            if (pool.Count > 0)
            {
                particles = pool.Dequeue();
                particles.transform.position = position;
            }
            else
            {
                particles = Instantiate(prefab, position, Quaternion.identity, particleContainer);
            }

            StartCoroutine(ReturnToPool(pool, particles, 2f));
            return particles;
        }

        private System.Collections.IEnumerator ReturnToPool(Queue<ParticleSystem> pool, ParticleSystem particles, float delay)
        {
            yield return new WaitForSeconds(delay);
            pool.Enqueue(particles);
        }
    }
}
