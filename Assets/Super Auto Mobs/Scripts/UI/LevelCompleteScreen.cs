﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class LevelCompleteScreen : Screen
    {
        public event Action OnClose;

        [SerializeField]
        private TextMeshProUGUI _titleText;

        [SerializeField]
        private TextMeshProUGUI _prizeText;
        
        [SerializeField]
        private Slider _healthSlider;
        
        [SerializeField]
        private Slider _progressSlider;

        [SerializeField]
        private Transform _prizeContainer;
        
        private LanguageService _languageService;
        private SessionProgressService _sessionProgressService;
        private List<Prize> _prizes = new();
        private DiContainer _diContainer;

        [Inject]
        private void Construct(LanguageService languageService, SessionProgressService sessionProgressService, DiContainer diContainer)
        {
            _languageService = languageService;
            _sessionProgressService = sessionProgressService;
            _diContainer = diContainer;
        }
        
        public void Open(EndBattleEnum endBattleEnum)
        {
            Open();
            var titleText = "";

            _healthSlider.maxValue = _sessionProgressService.CurrentWorld.MaxHealth;
            _healthSlider.value = _sessionProgressService.Hearts;
                    
            _progressSlider.maxValue = _sessionProgressService.CurrentWorld.MaxWins;
            _progressSlider.value = _sessionProgressService.Wins;

            _prizeText.gameObject.SetActive(_sessionProgressService.CurrentLevel.Prizes.Count > 0);
            
            foreach (var prefab in _sessionProgressService.CurrentLevel.Prizes)
            {
                var prize = Instantiate(prefab, _prizeContainer);
                _diContainer.Inject(prize);
                prize.Activate();
                _prizes.Add(prize);
            }
            
            switch (endBattleEnum)
            {
                case EndBattleEnum.Won:
                    titleText = "Won";

                    if (_sessionProgressService.Hearts < _sessionProgressService.CurrentWorld.MaxHealth)
                    {
                        _sessionProgressService.Hearts++;
                    }

                    _sessionProgressService.Wins++;
                    break;
                case EndBattleEnum.Lose:
                    titleText = "Lose";

                    _sessionProgressService.Hearts--;
                    break;
                case EndBattleEnum.Faint:
                    titleText = "Faint";
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(endBattleEnum), endBattleEnum, null);
            }

            _prizeText.text = "Prize";
            _titleText.text = "You " + titleText;
        }

        public override void Close()
        {
            for (int i = 0; i < _prizes.Count; i++)
            {
                Destroy(_prizes[i].gameObject);
            }

            _prizes = new List<Prize>();

            OnClose?.Invoke();
            base.Close();
        }
    }
}