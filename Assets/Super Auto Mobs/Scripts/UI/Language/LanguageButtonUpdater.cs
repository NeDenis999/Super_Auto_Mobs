using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class LanguageButtonUpdater : MonoBehaviour
    {
        [SerializeField]
        private List<LanguageService.Language> _languages;

        [SerializeField]
        private List<Sprite> _flagSprites;

        [SerializeField]
        private Image _flag;
        
        [SerializeField]
        private Button _button;

        private LanguageService _languageService;
        private int _currentNumberLanguage;

        [Inject]
        private void Construct(LanguageService languageService)
        {
            _languageService = languageService;
        }
        
        private void Awake()
        {
            _button.onClick.AddListener(UpdateLanguage);
            _languageService.OnUpdateLanguage += LoadLanguage;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(UpdateLanguage);
            _languageService.OnUpdateLanguage -= LoadLanguage;
        }

        private void Start()
        {
            LoadLanguage(_languageService.CurrentLanguage);
        }

        private void UpdateLanguage()
        {
            _currentNumberLanguage = MathfExtensions.RepeatInt(_currentNumberLanguage + 1, _languages.Count);
            _languageService.UpdateLanguage(_languages[_currentNumberLanguage]);
        }

        private void LoadLanguage(LanguageService.Language language)
        {
            _flag.sprite = _flagSprites[(int)language];
        }
    }
}