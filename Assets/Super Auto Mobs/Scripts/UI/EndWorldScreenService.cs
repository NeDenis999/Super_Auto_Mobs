using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Super_Auto_Mobs
{
    public class EndWorldScreenService : MonoBehaviour
    {
        [SerializeField]
        private Screen _endWorldScreen, _blackout;

        [SerializeField]
        private SessionProgressService _sessionProgressService;

        [SerializeField]
        private LanguageService _languageService;

        [SerializeField]
        private TextMeshProUGUI _titleText, _infoText;

        [SerializeField]
        private Title _winTitle, _winInfo, _loseTitle, _loseInfo;

        [SerializeField]
        private Button _button;
        
        private void OnEnable()
        {
            _sessionProgressService.OnUpdateInEndWorld += Open;
        }

        private void OnDisable()
        {
            _sessionProgressService.OnUpdateInEndWorld -= Open;
        }

        public void Open()
        {
            if (_sessionProgressService.Hearts > 0)
            {
                _titleText.text = _languageService.GetText(_winTitle);
                _infoText.text = _languageService.GetText(_winInfo);
            }
            else
            {
                _titleText.text = _languageService.GetText(_loseTitle);
                _infoText.text = _languageService.GetText(_loseInfo);
                _button.onClick.AddListener(Click);
            }
            
            _blackout.Open();
            _endWorldScreen.Open();
        }

        private void Click()
        {
            _button.onClick.RemoveListener(Click);
            _sessionProgressService.RemoveCurrentWorld();
        }
    }
}