using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PongClone
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _music = null;
        [SerializeField] private AudioSource _sound = null;
        public bool MusicOn
        {
            set { _music.mute = !value; }
        }
        public bool SoundOn
        {
            set { _sound.mute = !value; }
        }

        public void LoadSetting(GlobalData data)
        {
            MusicOn = data.MusicOn;
            SoundOn = data.SoundOn;
        }

        public void PlaySound(AudioClip clip)
        {
            if (!_sound.mute)
            {
                _sound.clip = clip;
                _sound.Play();
            }
        }

        public void PlaySoundRepeat(AudioClip clip, int times, float interval)
        {
            if (!_sound.mute)
            {
                StartCoroutine(PlaySoundRepeatTask(clip, times, interval));
            }
        }

        private IEnumerator PlaySoundRepeatTask(AudioClip clip, int times, float interval)
        {
            int count = 0;
            WaitForSeconds wait = new WaitForSeconds(interval);
            while (count++ < times)
            {
                _sound.PlayOneShot(clip);
                yield return wait;
            }
        }
    }
}