using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Game : MonoBehaviour
    {
        public event Action<GameState> OnUpdateGameState;

        [SerializeField]
        private GameState _startGameState;

        [SerializeField]
        private bool _isTest;

        [SerializeField]
        private World _world;

        private GameState _currentGameState;
        private GameState _previousGameState = GameState.None;
        private ShopService _shopService;
        private BattleService _battleService;
        private StartScreenService _startScreenService;
        private TitlesService _titlesService;
        private SessionProgressService _sessionProgressService;
        private LoadScreenService _loadScreenService;

        private bool _isLoad;

        public bool IsLoad => _isLoad;

        public GameState CurrentGameState
        {
            get => _currentGameState;
            set
            {
                if (_currentGameState == value)
                    return;

                _isLoad = true;
                StartCoroutine(AwaitCurrentGameState(value));
            }
        }

        [Inject]
        private void Construct(ShopService shopService, BattleService battleBaseService, StartScreenService startScreenService,
            TitlesService titlesService, SessionProgressService sessionProgressService, LoadScreenService loadScreenService)
        {
            _shopService = shopService;
            _battleService = battleBaseService;
            _startScreenService = startScreenService;
            _titlesService = titlesService;
            _sessionProgressService = sessionProgressService;
            _loadScreenService = loadScreenService;
        }

        private void Start()
        {
            CurrentGameState = GameState.StartMenu;
            
            if (_isTest)
            {
                _sessionProgressService.SetWorldData(_world.WorldData);
                CurrentGameState = _startGameState;
            }
        }

        private IEnumerator AwaitCurrentGameState(GameState gameState)
        {
            _previousGameState = _currentGameState;
            _currentGameState = gameState;
            
            switch (_previousGameState)
            {
                case GameState.None:
                    _startScreenService.Close();
                    _shopService.Close();
                    _battleService.Close();
                    _titlesService.Close();
                    break;
                case GameState.StartMenu:
                    if (!_isTest)
                        yield return _loadScreenService.Open();
                    
                    _startScreenService.Close();
                    break;
                case GameState.Shop:
                    if (!_isTest)
                        yield return _loadScreenService.Open();
                    _shopService.Close();
                    _sessionProgressService.Gold = 10;
                    break;
                case GameState.Battle:
                    if (!_isTest)
                        yield return _loadScreenService.Open();
                    _battleService.Close();
                    break;
                case GameState.Titles:
                    _titlesService.Close();
                    break;
                case GameState.World:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (_currentGameState)
            {
                case GameState.None:
                    break;
                case GameState.StartMenu:
                    _startScreenService.PreparationOpen();
                    
                    if (_previousGameState != GameState.None)
                        yield return _loadScreenService.Close();

                    _startScreenService.Open();
                    break;
                case GameState.Shop:
                    _shopService.Open();

                    if (!_isTest)
                        yield return _loadScreenService.Close();
                    break;
                case GameState.Battle:
                    _battleService.Open();
                    
                    if (!_isTest)
                        yield return _loadScreenService.Close();
                    StartCoroutine(_battleService.AwaitProcessBattle());
                    break;
                case GameState.Titles:
                    _titlesService.Open();
                    break;
                case GameState.World:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _isLoad = false;
            OnUpdateGameState?.Invoke(_currentGameState);
        }
    }
}