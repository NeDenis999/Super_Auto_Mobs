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
        
        [SerializeField]
        private float _tiemMove;
        
        [SerializeField]
        private float _startDelay;

        [SerializeField]
        private float _widhtSize;

        private LanguageService _languageService;

        [Inject]
        private void Construct(LanguageService languageService)
        {
            _languageService = languageService;
        }

        public override void Open()
        {
            base.Open();
            _languageService.OnUpdateLanguage += UpdateText;
            StartCoroutine(Play());
        }

        public override void Close()
        {
            if (!gameObject.activeSelf)
                return;
            
            StopCoroutine(Play());
            _languageService.OnUpdateLanguage -= UpdateText;
            base.Close();
        }
        
        public IEnumerator Play()
        {
            UpdateText(_languageService.CurrentLanguage);
            _text.transform.position = _text.transform.position.SetY(-UnityEngine.Screen.height / 2 - _widhtSize);
            yield return new WaitForSeconds(_startDelay);
            yield return LeanTween.moveY(_text.gameObject, UnityEngine.Screen.height + _widhtSize, _tiemMove);
        }

        private void UpdateText(LanguageService.Language language)
        {
            _text.text = _languageService.GetText(_title);

            if (DateTime.Now.Day == 8 && DateTime.Now.Month == 5)
            {
                _text.text += '\n' + '\n' + _languageService.GetText(_titleEasterEgg);
            }
        }
    }
}