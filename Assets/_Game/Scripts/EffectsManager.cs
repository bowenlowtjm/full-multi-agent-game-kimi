using UnityEngine;
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
        [SerializeField] private Transform particleContainer;

        [Header("Screenshake")]
        [SerializeField] private float shakeDuration = 0.2f;
        [SerializeField] private float shakeMagnitude = 0.3f;
        private float shakeTimer = 0f;
        private Vector3 originalCamPos;
        private Transform cameraTransform;

        [Header("Flash")]
        [SerializeField] private GameObject flashOverlay;
        [SerializeField] private float flashDuration = 0.1f;

        [Header("Floating Text")]
        [SerializeField] private GameObject scorePopupPrefab;
        [SerializeField] private Transform popupContainer;

        private Queue<ParticleSystem> hitParticlePool = new Queue<ParticleSystem>();
        private Queue<ParticleSystem> missParticlePool = new Queue<ParticleSystem>();

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

        public void PlayHitEffect(Vector3 position, Color color, int score)
        {
            // Spawn particles
            ParticleSystem particles = GetPooledParticle(hitParticlePool, hitParticles, position);
            if (particles != null)
            {
                particles.startColor = color;
                particles.Play();
            }

            // Floating score text
            if (scorePopupPrefab != null)
            {
                SpawnFloatingText(position, $"+{score}", color);
            }
        }

        public void PlayMissEffect(Vector3 position)
        {
            // Spawn particles
            ParticleSystem particles = GetPooledParticle(missParticlePool, missParticles, position);
            if (particles != null)
            {
                particles.startColor = Color.red;
                particles.Play();
            }

            // Screenshake
            ShakeScreen();

            // Flash red
            FlashScreen(Color.red);
        }

        public void PlayComboEffect(int combo)
        {
            if (comboParticles != null)
            {
                comboParticles.Play();
            }
        }

        public void ShakeScreen()
        {
            shakeTimer = shakeDuration;
        }

        public void FlashScreen(Color color)
        {
            if (flashOverlay != null)
            {
                SpriteRenderer sr = flashOverlay.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = color;
                    sr.gameObject.SetActive(true);
                    StartCoroutine(HideFlash());
                }
            }
        }

        private System.Collections.IEnumerator HideFlash()
        {
            yield return new WaitForSeconds(flashDuration);
            if (flashOverlay != null)
                flashOverlay.SetActive(false);
        }

        private void SpawnFloatingText(Vector3 position, string text, Color color)
        {
            // Implementation depends on having a floating text prefab
            // Simplified: just log for now
            Debug.Log($"[FX] {text} at {position}");
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

            // Return to pool when done
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
