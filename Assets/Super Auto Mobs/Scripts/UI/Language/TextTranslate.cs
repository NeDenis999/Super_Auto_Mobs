using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class TextTranslate : MonoBehaviour
    {
        public event Action OnTextUpdate;
        
        [SerializeField]
        private Title _title;
        
        private TextMeshProUGUI _textMeshPro;
        private LanguageService _languageService;

        [Inject]
        private void Construct(LanguageService languageService)
        {
            _languageService = languageService;
        }
        
        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _languageService.OnUpdateLanguage += UpdateLanguage;
            UpdateLanguage(_languageService.CurrentLanguage);
        }

        private void OnDestroy()
        {
            _languageService.OnUpdateLanguage -= UpdateLanguage;
        }
        
        private void UpdateLanguage(LanguageService.Language language)
        {
            _textMeshPro.text = _languageService.GetText(_title);
            OnTextUpdate?.Invoke();
        }
    }
}