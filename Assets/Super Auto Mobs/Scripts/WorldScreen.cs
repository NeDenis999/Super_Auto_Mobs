using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class WorldScreen : MonoBehaviour
    {
        [SerializeField]
        private World _world;

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private TextMeshProUGUI _healthText;
        
        [SerializeField]
        private TextMeshProUGUI _progressText;
        
        [SerializeField]
        private Image _previewImage;
        
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Slider _healthSlider;
        
        [SerializeField]
        private Slider _progressSlider;

        [SerializeField]
        private GameObject _end;
        
        private SessionProgressService _sessionProgressService;
        private LanguageService _languageService;
        private Game _game;
        private LoadScreenService _loadScreenService;
        private StartScreenService _startScreenService;
        private CoroutineRunner _coroutineRunner;

        public World World
        {
            get => _world;
            set
            {
                _world = value;
                _text.text = _languageService.GetText(value.WorldData.Title);
                _previewImage.sprite = value.Preview;
                
                var progress = _sessionProgressService.GetProgress(_world);
                _healthText.text = progress.Hearts.ToString();
                _progressText.text = $"{progress.Wins}/{_world.WorldData.LevelsData.Count}";
                _healthSlider.maxValue = _world.WorldData.MaxHealth;
                _healthSlider.value = progress.Hearts;
                _progressSlider.maxValue = _world.WorldData.LevelsData.Count;
                _progressSlider.value = progress.Wins;
            }
        }

        [Inject]
        private void Construct(SessionProgressService sessionProgressService, LanguageService languageService, Game game, 
            LoadScreenService loadScreenService, StartScreenService startScreenService, CoroutineRunner coroutineRunner)
        {
            _sessionProgressService = sessionProgressService;
            _languageService = languageService;
            _game = game;
            _loadScreenService = loadScreenService;
            _startScreenService = startScreenService;
            _coroutineRunner = coroutineRunner;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OpenWorld);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OpenWorld);
        }

        private void OpenWorld()
        {
            if (_sessionProgressService.GetProgress(_world).IsEndWorld)
            {
                _startScreenService.warningWindow.Open(_world);
            }
            else
            {
                _game.OpenWorld(_world);
                //_startScreenService.OpenWorld(_world);
            }
        }

        public void UpdateView()
        {
            _end.gameObject.SetActive(_sessionProgressService.GetProgress(_world).IsEndWorld);
        }
    }
}