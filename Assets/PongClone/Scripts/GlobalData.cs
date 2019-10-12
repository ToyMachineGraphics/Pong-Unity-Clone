using System.Collections.Generic;
using UnityEngine;

namespace PongClone
{
    public class GlobalData : MonoBehaviour
    {
        private static GlobalData _instance;
        public static GlobalData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GlobalData>();
                    if (_instance == null)
                    {
                        _instance = (new GameObject("GlobalData")).AddComponent<GlobalData>();
                        _instance._saveData = SaveData.Load();
                        DontDestroyOnLoad(_instance.gameObject);
                    }
                }
                return _instance;
            }
        }

        #region Save Data
#if UNITY_EDITOR
        public bool saveDataFromDisk = true;
#endif
        [SerializeField] private SaveData _saveData;
        public int PointsToWin
        {
            get { return _saveData.pointsTowin; }
            set { _saveData.pointsTowin = value; }
        }
        public Handedness Handedness
        {
            get { return _saveData.handedness; }
            set { _saveData.handedness = value; }
        }
        public bool MusicOn
        {
            get { return _saveData.musicOn; }
            set { _saveData.musicOn = _audioManager.MusicOn = value; }
        }
        public bool SoundOn
        {
            get { return _saveData.soundOn; }
            set { _saveData.soundOn = _audioManager.SoundOn = value; }
        }
        public List<int> TopPoints
        {
            get { return _saveData.localTopPoints; }
        }
        public int LastSessionPoint
        {
            get { return _saveData.lastSessionPoint; }
            set { _saveData.lastSessionPoint = value; }
        }

        public int InsertToBillboard(int point)
        {
            return _saveData.InsertToBillboard(point);
        }
        #endregion

        #region Audio
        private AudioManager _audioManager;
        [SerializeField] private List<AudioClip> _ballHit = null;
        [SerializeField] private AudioClip _deathPulse = null;
        [SerializeField] private AudioClip _uiClick = null;
        #endregion

        private void Awake()
        {
            if (_instance == null)
            {
                Debug.LogFormat("GlobalData created: {0}", GetInstanceID());
                _instance = this;
#if UNITY_EDITOR
                if (saveDataFromDisk)
                {
                    _saveData = SaveData.Load();
                }
#else
                _saveData = SaveData.Load();
#endif
                _audioManager = GetComponent<AudioManager>();
                _audioManager.LoadSetting(this);
                DontDestroyOnLoad(transform.root.gameObject);
            }
            else
            {
                if (_instance.gameObject != gameObject)
                {
                    Debug.LogFormat("GlobalData destroyed: {0}", GetInstanceID());
                    Destroy(gameObject);
                }
            }
        }

        #region Test
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                _saveData.Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                _saveData = SaveData.Load();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                SaveData.Clear();
                _saveData = new SaveData();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                _audioManager.PlaySoundRepeat(_deathPulse, 5, 0.3f);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                _audioManager.PlaySound(_ballHit[0]);
            }
        }
        #endregion

        #region Audio
        public void PlayUIClickSound()
        {
            _audioManager.PlaySound(_uiClick);
        }

        public void PlayBallHitSoundWith(GameObject with)
        {
            int id = with.CompareTag(Definition.WALL) ? 1 : 0;
            _audioManager.PlaySound(_ballHit[id]);
        }

        public void PlayDeathSound()
        {
            _audioManager.PlaySoundRepeat(_deathPulse, 5, 0.3f);
        }
        #endregion

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus && _instance.gameObject == gameObject)
            {
                _saveData.Save();
                SaveData.WriteToDisk();
            }
        }
#else
        private void OnApplicationQuit()
        {
            if (_instance.gameObject == gameObject)
            {
                _saveData.Save();
                SaveData.WriteToDisk();
                _instance = null;
            }
        }
#endif
    }
}