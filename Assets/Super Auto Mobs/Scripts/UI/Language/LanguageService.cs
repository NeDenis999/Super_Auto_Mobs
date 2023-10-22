using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class LanguageService : MonoBehaviour
    {
        [SerializeField]
        private SessionProgressService _sessionProgressService;
        
        public enum Language
        {
            English,
            Russian
        }
    
        public event Action<Language> OnUpdateLanguage;

        public Language CurrentLanguage => _sessionProgressService.Language;

        /*[Inject]
        private void Construct(SessionProgressService sessionProgressService)
        {
            _sessionProgressService = sessionProgressService;
        }*/
        
        private void Start()
        {
            OnUpdateLanguage?.Invoke(CurrentLanguage);
        }

        public void UpdateLanguage(Language language)
        {
            _sessionProgressService.Language = language;
            OnUpdateLanguage?.Invoke(language);
            
            /*
            foreach (var contentSizeFitter in FindObjectsOfType<ContentSizeFitter>())
            {
                contentSizeFitter.SetLayoutHorizontal();
                contentSizeFitter.SetLayoutVertical();
            }*/
        }

        public string GetText(Title title)
        {
            switch (CurrentLanguage)
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
