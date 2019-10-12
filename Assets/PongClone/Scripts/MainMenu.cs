using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PongClone
{
    public class MainMenu : MonoBehaviour
    {
        public GlobalData globalData;
        //[SerializeField] private SceneFader _screenFade;
        public SceneFader sceneFader;

        [SerializeField] private BillboardPage _billboard = null;
        public BillboardPage Billboard
        {
            get { return _billboard; }
        }
        [SerializeField] private GameObject _billboardBack = null;

        private void Awake()
        {
            _billboard.back = _billboardBack;
        }

        private void Start()
        {
            InitSettingPage();
        }

        private void OnDestroy()
        {
            Debug.LogFormat("MainMenu: OnDestroy");
        }

        public void LoadSceneAsync(string sceneName)
        {
            sceneFader.LoadSceneAsync(sceneName);
        }

        public void PlayUIClickSound()
        {
            globalData.PlayUIClickSound();
        }

        #region Setting Page
        [SerializeField] private RectTransform _pointsToWinParent = null;
        [SerializeField] private RectTransform _handednessParent = null;
        [SerializeField] private Toggle _music = null;
        [SerializeField] private Toggle _sound = null;

        private void InitSettingPage()
        {
            // Prevent ui from makeing click sounds on initialization.
            bool soundOn = globalData.SoundOn;
            globalData.SoundOn = false;

            // 1/3/5
            for (int i = 0; i < _pointsToWinParent.childCount; i++)
            {
                Toggle t = _pointsToWinParent.GetChild(i).GetComponent<Toggle>();
                int index = i;
                t.onValueChanged.AddListener((b) =>
                {
                    SetPointsToWin(b, 1 + 2 * index);
                });
                t.isOn = false;
            }
            _pointsToWinParent.GetChild((globalData.PointsToWin - 1) / 2).GetComponent<Toggle>().isOn = true;
            _handednessParent.GetChild(0).GetComponent<Toggle>().onValueChanged.AddListener((b) =>
            {
                SetHandedness(b, Handedness.Left);
            });
            _handednessParent.GetChild(0).GetComponent<Toggle>().isOn = (globalData.Handedness == Handedness.Left);
            _handednessParent.GetChild(1).GetComponent<Toggle>().onValueChanged.AddListener((b) =>
            {
                SetHandedness(b, Handedness.Right);
            });
            _handednessParent.GetChild(1).GetComponent<Toggle>().isOn = (globalData.Handedness == Handedness.Right);

            // Restore data setting.
            globalData.SoundOn = soundOn;
            _music.isOn = globalData.MusicOn;
            _sound.isOn = globalData.SoundOn;
        }

        public void SetPointsToWin(bool isOn, int pointToWin)
        {
            PlayUIClickSound();
            if (isOn)
            {
                globalData.PointsToWin = pointToWin;
            }
        }

        public void SetHandedness(bool isOn, Handedness handedness)
        {
            PlayUIClickSound();
            if (isOn)
            {
                globalData.Handedness = handedness;
            }
        }

        public void ToggleMusic(bool on)
        {
            globalData.MusicOn = on;
        }

        public void ToggleSound(bool on)
        {
            globalData.SoundOn = on;
        }
        #endregion

        #region Billboard Page
        public void PlayBillboard()
        {
            Billboard.PlayBillboard();
        }
        #endregion
    }
}