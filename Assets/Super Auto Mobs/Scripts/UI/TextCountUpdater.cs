using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{

    public class TextCountUpdater : MonoBehaviour
    {
        [SerializeField]
        private CountData _countData;
        
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
        }

        private void OnEnable()
        {
            switch (_countData)
            {
                case CountData.Emeralds:
                    _sessionProgressService.OnUpdateEmeralds += TextUpdate;
                    break;
                case CountData.Hearts:
                    _sessionProgressService.OnUpdateHearts += TextUpdate;
                    break;
                case CountData.Wins:
                    _sessionProgressService.OnUpdateWins += TextUpdate;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDisable()
        {
            switch (_countData)
            {
                case CountData.Emeralds:
                    _sessionProgressService.OnUpdateEmeralds -= TextUpdate;
                    break;
                case CountData.Hearts:
                    _sessionProgressService.OnUpdateHearts -= TextUpdate;
                    break;
                case CountData.Wins:
                    _sessionProgressService.OnUpdateWins -= TextUpdate;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TextUpdate(int value)
        {
            _text.text = value.ToString();
        }
        
        private void TextUpdate(int value, int maxValue)
        {
            _text.text = $"{value}/{maxValue}";
        }
    }
}