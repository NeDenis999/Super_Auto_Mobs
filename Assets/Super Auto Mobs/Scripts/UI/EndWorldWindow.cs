using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Super_Auto_Mobs
{
    public class EndWorldWindow : BaseWindow
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
            }
            
            _blackout.Open();
            _endWorldScreen.Open();
        }
    }
}