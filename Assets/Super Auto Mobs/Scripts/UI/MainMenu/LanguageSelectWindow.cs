using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class LanguageSelectWindow : BaseWindow
    {
        [SerializeField]
        private List<FlagButton> _flagButtons;

        private SessionProgressService _sessionProgressService;
        private LanguageService _languageService;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService, LanguageService languageService)
        {
            _languageService = languageService;
            _sessionProgressService = sessionProgressService;
        }
        
        private void OnEnable()
        {
            foreach (var flag in _flagButtons)
            {
                flag.OnClick += Click;
            }
        }

        private void OnDisable()
        {
            foreach (var flag in _flagButtons)
            {
                flag.OnClick -= Click;
            }
        }

        private void Start()
        {
            Click(_flagButtons[0]);
        }

        private void Click(FlagButton flagButton)
        {
            foreach (var flag in _flagButtons)
            {
                flag.Off();
            }

            flagButton.On();
            _languageService.UpdateLanguage(flagButton.Language);
        }
    }
}