using UnityEngine;
using UnityEngine.Audio;

namespace Super_Auto_Mobs
{
    public class SoundsService : MonoBehaviour
    {
        [SerializeField]
        private SessionProgressService _sessionProgressService;
        
        [SerializeField]
        private AudioMixer _audioMixer;
        
        [SerializeField]
        private AudioSource _audioSourceMusic;
        
        [SerializeField]
        private AudioSource _audioSourceSound;
        
        [Header("Sounds")]
        [SerializeField]
        private AudioClip _buySound;
        
        [SerializeField]
        private AudioClip _winSound;
        
        [SerializeField]
        private AudioClip _clickSound;
        
        [SerializeField]
        private AudioClip _eatSound;
        
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
            UpdateMusicVolume(_sessionProgressService.Music);
            UpdateSoundVolume(_sessionProgressService.Sound);
        }

        private void OnEnable()
        {
            _sessionProgressService.OnUpdateMusic += UpdateMusicVolume;
            _sessionProgressService.OnUpdateSound += UpdateSoundVolume;
        }
        
        private void OnDisable()
        {
            _sessionProgressService.OnUpdateMusic -= UpdateMusicVolume;
            _sessionProgressService.OnUpdateSound -= UpdateSoundVolume;
        }

        private void UpdateMusicVolume(float value)
        {
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 30);
        }

        private void UpdateSoundVolume(float value)
        {
            _audioMixer.SetFloat("SoundVolume", Mathf.Log10(value) * 30);
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

        public void PlayWin()
        {
            PlaySound(_winSound);
        }

        public void PlayClick()
        {
            PlaySound(_clickSound);
        }
        
        public void PlayEat()
        {
            PlaySound(_eatSound);
        }

        public void PlayBuy()
        {
            PlaySound(_buySound);
        }
    }
}