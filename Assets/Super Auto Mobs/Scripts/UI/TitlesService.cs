using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class TitlesService : Screen
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private Title _title;
        
        [SerializeField]
        private Title _titleEasterEgg;

        private LanguageService _languageService;

        [Inject]
        private void Construct(LanguageService languageService)
        {
            _languageService = languageService;
        }

        private void OnEnable()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            _text.text = _languageService.GetText(_title);

            if (DateTime.Now.Day == 8 && DateTime.Now.Month == 5)
            {
                _text.text += '\n' + '\n' + _languageService.GetText(_titleEasterEgg);
            }
        }
    }
}