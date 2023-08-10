using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{

    public class TextCountUpdater : MonoBehaviour
    {
        [SerializeField]
        private CountEnum countEnum;
        
        private SessionProgressService _sessionProgressService;
        private TextMeshProUGUI _text;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService)
        {
            _sessionProgressService = sessionProgressService;
        }

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            
            switch (countEnum)
            {
                case CountEnum.Emeralds:
                    _sessionProgressService.OnUpdateEmeralds += TextUpdate;
                    TextUpdate(_sessionProgressService.Gold);
                    break;
                case CountEnum.Hearts:
                    _sessionProgressService.OnUpdateHearts += TextUpdate;
                    TextUpdate(_sessionProgressService.Hearts);
                    break;
                case CountEnum.Wins:
                    _sessionProgressService.OnUpdateWins += WinsTextUpdate;
                    TextUpdate(_sessionProgressService.Wins);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDestroy()
        {
            switch (countEnum)
            {
                case CountEnum.Emeralds:
                    _sessionProgressService.OnUpdateEmeralds -= TextUpdate;
                    break;
                case CountEnum.Hearts:
                    _sessionProgressService.OnUpdateHearts -= TextUpdate;
                    break;
                case CountEnum.Wins:
                    _sessionProgressService.OnUpdateWins -= WinsTextUpdate;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TextUpdate(int value)
        {
            _text.text = value.ToString();
        }
        
        private void WinsTextUpdate(int value)
        {
            _text.text = $"{value}/{_sessionProgressService.CurrentWorldData.LevelsData.Count}";
        }
    }
}