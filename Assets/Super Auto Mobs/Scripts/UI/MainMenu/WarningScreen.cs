using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Super_Auto_Mobs
{
    public class WarningScreen : MonoBehaviour
    {
        [SerializeField]
        private Screen _screen;

        [SerializeField]
        private StartScreenService _startScreenService;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private LanguageService _languageService;

        [SerializeField]
        private SessionProgressService _sessionProgressService;
        
        [SerializeField]
        private TextMeshProUGUI _text;
        
        [SerializeField]
        private Title _loseTitle, _winTitle;
        
        private World _world;

        private void OnEnable()
        {
            _button.onClick.AddListener(OpenWorld);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OpenWorld);
        }

        public void Open(World world)
        {
            _world = world;

            if (_sessionProgressService.GetProgress(_world).Hearts > 0)
            {
                _text.text = _languageService.GetText(_winTitle);
            }
            else
            {
                _text.text = _languageService.GetText(_loseTitle);
            }
            
            _screen.Open();
        }

        private void OpenWorld()
        {
            _sessionProgressService.RemoveCurrentWorld();
            _startScreenService.OpenWorld(_world);
        }
    }
}