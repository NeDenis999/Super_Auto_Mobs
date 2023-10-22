using System;
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
        private AudioClip _eatSound,_itemSound, _potionSound, _potionExplosionSound;
        
        [Header("Music")]
        [SerializeField]
        private AudioClip _mainMenuTheme;

        [SerializeField]
        private AudioClip _shopTheme;
        
        [SerializeField]
        private AudioClip _battleTheme;

        [SerializeField]
        private AudioClip _speedRun;
        
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

        public void StopMusic() =>
            _audioSourceMusic.Stop();

        public void PlayDreamSpeedrun() =>
            PlayMusic(_speedRun);

        public void PlayMenu() =>
            PlayMusic(_mainMenuTheme);

        public void PlayShop() =>
            PlayMusic(_shopTheme);

        public void PlayBattle() =>
            PlayMusic(_battleTheme);

        public void PlayWin() =>
            PlaySound(_winSound);

        public void PlayClick() =>
            PlaySound(_clickSound);

        public void PlayEat(BuffSoundEnum buffDataBuffSoundEnum)
        {
            switch (buffDataBuffSoundEnum)
            {
                case BuffSoundEnum.Food:
                    PlaySound(_eatSound);
                    break;
                case BuffSoundEnum.Item:
                    PlaySound(_itemSound);
                    break;
                case BuffSoundEnum.Potion:
                    PlaySound(_potionSound);
                    break;
                case BuffSoundEnum.PotionExplosion:
                    PlaySound(_potionExplosionSound);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buffDataBuffSoundEnum), buffDataBuffSoundEnum, null);
            }
        }

        public void PlayBuy() =>
            PlaySound(_buySound);
    }
}