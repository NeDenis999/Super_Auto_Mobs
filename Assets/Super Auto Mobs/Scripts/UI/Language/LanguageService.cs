using System;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class LanguageService : MonoBehaviour
    {
        public enum Language
        {
            English,
            Russian
        }
    
        public event Action<Language> OnUpdateLanguage;

        public Language CurrentLanguage => _currentLanguage;

        private Language _currentLanguage;

        private void Start()
        {
            OnUpdateLanguage?.Invoke(Language.English);
        }

        public void UpdateLanguage(Language language)
        {
            _currentLanguage = language;
            OnUpdateLanguage?.Invoke(language);
        }

        public string GetText(Title title)
        {
            switch (_currentLanguage)
            {
                case Language.English:
                    return title.English;
                case Language.Russian:
                    return title.Russian;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
