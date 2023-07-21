using System;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class SoundsService : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSourceMusic;
        
        [SerializeField]
        private AudioSource _audioSourceSound;
        
        [Header("Source")]
        [SerializeField]
        private AudioClip _buySound;
        
        [Header("Music")]
        [SerializeField]
        private AudioClip _mainTheme;
        
        [SerializeField]
        private AudioClip _speedRun;
        
        [SerializeField]
        private AudioClip _battleTheme;

        private void Start()
        {
            //PlayMusic(_speedRun);
        }

        private void PlayMusic(AudioClip music)
        {
            _audioSourceMusic.clip = music;
            _audioSourceMusic.Play();
        }
        
        private void PlaySound(AudioClip sound)
        {
            _audioSourceSound.clip = sound;
            _audioSourceSound.Play();
        }

        public void StopMusic()
        {
            _audioSourceMusic.Stop();
        }

        public void PlayDreamSpeedrun()
        {
            PlayMusic(_speedRun);
        }
    }
}