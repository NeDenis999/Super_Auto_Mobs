using System.Collections.Generic;
using Super_Auto_Mobs.Scripts.Extensions;
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
        
        private void OnEnable()
        {
            _button.onClick.AddListener(UpdateLanguage);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(UpdateLanguage);
        }

        private void UpdateLanguage()
        {
            _currentNumberLanguage = MathfExtensions.RepeatInt(_currentNumberLanguage + 1, _languages.Count);
            _languageService.UpdateLanguage(_languages[_currentNumberLanguage]);
            _flag.sprite = _flagSprites[_currentNumberLanguage];
        }
    }
}