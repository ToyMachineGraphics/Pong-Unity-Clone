using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PongClone
{
    public class SceneFader : MonoBehaviour
    {
        private static SceneFader _instance;
        public static SceneFader Instance
        {
            get { return _instance; }
        }

        #region Fade
        public bool FadedIn { get; private set; }
        public bool FadedOut { get; private set; }

        [SerializeField] Image _cover = null;
        [SerializeField] private float fadeInDuration = 1;
        [SerializeField] private float fadeOutDuration = 1;
        #endregion

        private void Awake()
        {
            if (_instance == null)
            {
                Debug.LogFormat("SceneFader Awake: {0}", GetInstanceID());
                _instance = this;
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.sceneUnloaded += ScreenFadeOut;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (_instance.gameObject != gameObject)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void Start()
        {
            _cover.canvasRenderer.SetAlpha(0);
        }

        private void OnDestroy()
        {
            Debug.LogFormat("SceneFader OnDestroy: {0}", GetInstanceID());
            if (_instance.gameObject == gameObject)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                SceneManager.sceneUnloaded -= ScreenFadeOut;
                _instance = null;
            }
        }

        #region Scene
        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadSceneTask(sceneName));
            FadeIn();
        }

        private IEnumerator LoadSceneTask(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;
            while (asyncLoad.progress < 0.9f || !FadedIn)
            {
                yield return null;
            }
            asyncLoad.allowSceneActivation = true;
            while (!asyncLoad.isDone)
            {
                Debug.LogFormat("isDone:{0}, progress:{1}", asyncLoad.isDone, asyncLoad.progress);
                yield return null;
            }
            Debug.LogFormat("isDone:{0}, progress:{1}", asyncLoad.isDone, asyncLoad.progress);
        }

        private void ScreenFadeOut(Scene current)
        {
            FadeOut();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.LogFormat("OnSceneLoaded: {0}", scene.name);
            Gameplay gameplay = FindObjectOfType<Gameplay>();
            if (gameplay)
            {
                gameplay.sceneFader = this;
                gameplay.globalData = GlobalData.Instance;
                gameplay.rightHandedness = (gameplay.globalData.Handedness == Handedness.Right);
                gameplay.ManualStart();
            }
            else if (scene.buildIndex == 0)
            {
                MainMenu mainMenu = FindObjectOfType<MainMenu>();
                mainMenu.sceneFader = this;
                mainMenu.Billboard.globalData = mainMenu.globalData = GlobalData.Instance;
            }
        }
        #endregion

        #region Fade
        public void FadeIn(Action onComplete = null)
        {
            FadedIn = false;
            StartCoroutine(FadeInTask(onComplete));
        }

        private IEnumerator FadeInTask(Action onComplete)
        {
            _cover.raycastTarget = true;
            _cover.CrossFadeAlpha(1, fadeInDuration, false);
            yield return new WaitUntil(() => _cover.canvasRenderer.GetAlpha() == 1);
            FadedIn = true;
            onComplete?.Invoke();
        }

        public void FadeOut(Action onComplete = null)
        {
            FadedOut = false;
            StartCoroutine(FadeOutTask(onComplete));
        }

        private IEnumerator FadeOutTask(Action onComplete)
        {
            _cover.CrossFadeAlpha(0, fadeOutDuration, false);
            yield return new WaitUntil(() => _cover.canvasRenderer.GetAlpha() == 0);
            _cover.raycastTarget = false;
            FadedOut = true;
            onComplete?.Invoke();
        }
        #endregion
    }
}