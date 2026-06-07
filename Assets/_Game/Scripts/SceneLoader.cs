using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arcade.Game
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        [Header("Loading")]
        [SerializeField] private GameObject loadingScreen;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (loadingScreen != null)
                loadingScreen.SetActive(false);
        }

        public void LoadScene(string sceneName)
        {
            if (loadingScreen != null)
                loadingScreen.SetActive(true);

            SceneManager.LoadScene(sceneName);
        }

        public void LoadSceneAsync(string sceneName)
        {
            if (loadingScreen != null)
                loadingScreen.SetActive(true);

            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
