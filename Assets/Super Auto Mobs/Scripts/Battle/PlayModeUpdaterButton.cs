using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class PlayModeUpdaterButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Image _iconImage;
        
        [SerializeField]
        private Sprite _playSprite;

        [SerializeField]
        private Sprite _autoPlaySprite;
        
        private SessionProgressService _sessionProgressService;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService)
        {
            _sessionProgressService = sessionProgressService;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(UpdatePlayMod);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(UpdatePlayMod);
        }

        private void Start()
        {
            UpdateView();
        }
        
        private void UpdatePlayMod()
        {
            _sessionProgressService.IsAutoPlay = !_sessionProgressService.IsAutoPlay;
            UpdateView();
        }

        private void UpdateView()
        {
            if (_sessionProgressService.IsAutoPlay)
            {
                _iconImage.sprite = _autoPlaySprite;
            }
            else
            {
                _iconImage.sprite = _playSprite;
            }
        }
    }
}