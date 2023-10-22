using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class ClickSound : MonoBehaviour
    {
        private Button _button;
        private SoundsService _soundsService;

        [Inject]
        private void Construct(SoundsService soundsService)
        {
            _soundsService = soundsService;
        }
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(PlaySoundClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(PlaySoundClick);
        }

        private void PlaySoundClick()
        {
            _soundsService.PlayClick();
        }
    }
}