using System;
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
        private TextMeshProUGUI _heartsText;
        
        [SerializeField]
        private TextMeshProUGUI _progressText;
        
        [SerializeField]
        private Slider _healthSlider;
        
        [SerializeField]
        private Slider _progressSlider;

        [SerializeField]
        private Transform _prizeContainer;

        [Header("Titles")]
        [SerializeField]
        private Title _winTitle;
        
        [SerializeField]
        private Title _loseTitle, _faintTitle, _youTitle, _prizeTitle;
        
        private LanguageService _languageService;
        private SessionProgressService _sessionProgressService;
        private List<Prize> _prizes = new();
        private DiContainer _diContainer;
        private SoundsService _soundsService;

        [Inject]
        private void Construct(LanguageService languageService, SessionProgressService sessionProgressService, 
            DiContainer diContainer, SoundsService soundsService)
        {
            _languageService = languageService;
            _sessionProgressService = sessionProgressService;
            _diContainer = diContainer;
            _soundsService = soundsService;
        }
        
        public void Open(EndBattleEnum endBattleEnum)
        {
            Open();
            var titleText = "";

            var startHearts = _sessionProgressService.Hearts;
            var startProgress = _sessionProgressService.Wins;

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
                    titleText = _languageService.GetText(_winTitle);
                    _soundsService.PlayWin();
                    _sessionProgressService.Hearts++;
                    _sessionProgressService.Wins++;
                    _sessionProgressService.IndexCurrentLevel++;
                    break;
                case EndBattleEnum.Lose:
                    titleText = _languageService.GetText(_loseTitle);
                    
                    if (_sessionProgressService.CurrentLevel.IsCanLose)
                    {
                        _sessionProgressService.IndexCurrentLevel++;
                    }
                    else
                    {
                        _sessionProgressService.Hearts--;
                    }

                    break;
                case EndBattleEnum.Faint:
                    titleText = _languageService.GetText(_faintTitle);
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(endBattleEnum), endBattleEnum, null);
            }

            _sessionProgressService.Turn += 1;
            
            _prizeText.text = _languageService.GetText(_prizeTitle);
            _titleText.text = _languageService.GetText(_youTitle) + " " + titleText;
            
            _healthSlider.maxValue = _sessionProgressService.CurrentWorldData.MaxHealth;
            _healthSlider.value = startHearts;
            _heartsText.text = $"{startHearts}/{_sessionProgressService.CurrentWorldData.MaxHealth}";  
            LeanTween.value(gameObject, startHearts, _sessionProgressService.Hearts, 1)
                .setOnUpdate(value =>
                {
                    _healthSlider.value = value;
                })                
                .setOnComplete(() =>
                {
                    _heartsText.text = $"{_sessionProgressService.Hearts}/{_sessionProgressService.CurrentWorldData.MaxHealth}";
                });

            _progressSlider.maxValue = _sessionProgressService.CurrentWorldData.LevelsData.Count;
            _progressSlider.value = startProgress;
            _progressText.text = $"{startProgress}/{_sessionProgressService.CurrentWorldData.LevelsData.Count}";   
            LeanTween.value(gameObject, startProgress, _sessionProgressService.Wins, 1)
                .setOnUpdate(value =>
                {
                    _progressSlider.value = value;
                })
                .setOnComplete(() =>
                {
                    _progressText.text = $"{_sessionProgressService.Wins}/{_sessionProgressService.CurrentWorldData.LevelsData.Count}";
                });
        }

        public override void Close()
        {
            LeanTween.cancel(gameObject);
            
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