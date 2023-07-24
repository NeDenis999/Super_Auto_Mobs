using System;
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
        
        private GameState _currentGameState;
        private GameState _previousGameState = GameState.None;
        private ShopService _shopService;
        private BattleService _battleService;
        private MainMenuService _mainMenuService;
        private TitlesService _titlesService;
        private SessionProgressService _sessionProgressService;
        
        public GameState CurrentGameState
        {
            get => _currentGameState;
            set
            {
                if (_currentGameState == value)
                    return;
                
                _previousGameState = _currentGameState;
                _currentGameState = value;

                if (!_isTest)
                {
                    switch (_previousGameState)
                    {
                        case GameState.None:
                            _mainMenuService.Close();
                            _shopService.Close();
                            _battleService.Close();
                            _titlesService.Close();
                            break;
                        case GameState.StartMenu:
                            _mainMenuService.Close();
                            break;
                        case GameState.ShopTransition:
                            break;
                        case GameState.Shop:
                            _shopService.Close();
                            _sessionProgressService.Gold = 10;
                            break;
                        case GameState.Battle:
                            _battleService.Close();
                            break;
                        case GameState.BattleTransition:
                            break;
                        case GameState.Titles:
                            _titlesService.Close();
                            break;
                        case GameState.World:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                
                OnUpdateGameState?.Invoke(_currentGameState);

                switch (_currentGameState)
                {
                    case GameState.None:
                        break;
                    case GameState.StartMenu:
                        _mainMenuService.Open();
                        break;
                    case GameState.ShopTransition:
                        break;
                    case GameState.Shop:
                        _shopService.Open();
                        break;
                    case GameState.BattleTransition:
                        _battleService.Open();
                        break;
                    case GameState.Battle:
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
            }
        }

        [Inject]
        private void Construct(ShopService shopService, BattleService battleBaseService, MainMenuService mainMenuService,
            TitlesService titlesService, SessionProgressService sessionProgressService)
        {
            _shopService = shopService;
            _battleService = battleBaseService;
            _mainMenuService = mainMenuService;
            _titlesService = titlesService;
            _sessionProgressService = sessionProgressService;
        }

        private void Start()
        {
            CurrentGameState = _startGameState;
        }
    }
}