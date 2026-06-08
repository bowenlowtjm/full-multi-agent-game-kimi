using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Arcade.Game
{
    /// <summary>
    /// Splash screen with async loading into main menu.
    /// </summary>
    public class SplashScreen : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Text titleText;
        [SerializeField] private Text subtitleText;
        [SerializeField] private Image logoImage;
        [SerializeField] private Image loadingBar;
        [SerializeField] private Text loadingText;

        [Header("Timing")]
        [SerializeField] private float minDisplayTime = 2f;
        [SerializeField] private float fadeInDuration = 0.5f;
        [SerializeField] private float fadeOutDuration = 0.5f;

        [Header("Next Scene")]
        [SerializeField] private string nextSceneName = "MainMenu";

        private AsyncOperation loadOperation;
        private float displayTimer = 0f;
        private bool isLoading = false;

        private void Start()
        {
            // Initialize UI
            SetAlpha(titleText, 0f);
            SetAlpha(subtitleText, 0f);
            if (logoImage != null) SetAlpha(logoImage, 0f);
            if (loadingBar != null) loadingBar.fillAmount = 0f;
            if (loadingText != null) loadingText.text = "Loading...";

            // Start async load
            StartCoroutine(LoadNextScene());

            // Fade in
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            float timer = 0f;
            while (timer < fadeInDuration)
            {
                timer += Time.deltaTime;
                float alpha = timer / fadeInDuration;
                SetAlpha(titleText, alpha);
                SetAlpha(subtitleText, alpha);
                if (logoImage != null) SetAlpha(logoImage, alpha);
                yield return null;
            }

            SetAlpha(titleText, 1f);
            SetAlpha(subtitleText, 1f);
            if (logoImage != null) SetAlpha(logoImage, 1f);
        }

        private IEnumerator LoadNextScene()
        {
            isLoading = true;
            displayTimer = 0f;

            // Start async load
            loadOperation = SceneManager.LoadSceneAsync(nextSceneName);
            loadOperation.allowSceneActivation = false;

            // Wait for minimum display time and load to complete
            while (displayTimer < minDisplayTime || loadOperation.progress < 0.9f)
            {
                displayTimer += Time.deltaTime;

                // Update loading bar
                if (loadingBar != null)
                {
                    float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                    loadingBar.fillAmount = progress;
                }

                yield return null;
            }

            // Fade out
            yield return StartCoroutine(FadeOut());

            // Activate scene
            loadOperation.allowSceneActivation = true;
        }

        private IEnumerator FadeOut()
        {
            float timer = 0f;
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

            while (timer < fadeOutDuration)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = 1f - (timer / fadeOutDuration);
                yield return null;
            }

            canvasGroup.alpha = 0f;
        }

        private void SetAlpha(Graphic graphic, float alpha)
        {
            if (graphic == null) return;
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }
}
