using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Game : MonoBehaviour
    {
        public event Action<GameState> OnUpdateGameState;

        [SerializeField]
        private bool _isTest;

        [SerializeField]
        private GameState _startGameState;

        [SerializeField]
        private bool _isWorldData;
        
        [SerializeField]
        private World _world;

        [SerializeField]
        private WorldData _worldData;

        [SerializeField]
        private int _indexCurrentLevel;

        [SerializeField]
        private bool _isSkipDialogs;

        private GameState _currentGameState;
        private GameState _previousGameState = GameState.None;
        private ShopService _shopService;
        private BattleService _battleService;
        private StartScreenService _startScreenService;
        private TitlesService _titlesService;
        private SessionProgressService _sessionProgressService;
        private LoadScreenService _loadScreenService;

        private bool _isLoad;
        private DialogService _dialogService;
        private BackgroundService _backgroundService;
        private CutScenesService _cutScenesService;
        private MenuService _menuService;
        private EndWorldScreenService _endWorldScreenService;

        public bool IsLoad => _isLoad;
        public bool IsTest => _isTest;

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
            TitlesService titlesService, SessionProgressService sessionProgressService, LoadScreenService loadScreenService,
            DialogService dialogService, BackgroundService backgroundService, CutScenesService cutScenesService, 
            MenuService menuService, EndWorldScreenService endWorldScreenService)
        {
            _endWorldScreenService = endWorldScreenService;
            _cutScenesService = cutScenesService;
            _backgroundService = backgroundService;
            _dialogService = dialogService;
            _shopService = shopService;
            _battleService = battleBaseService;
            _startScreenService = startScreenService;
            _titlesService = titlesService;
            _sessionProgressService = sessionProgressService;
            _loadScreenService = loadScreenService;
            _menuService = menuService;
        }

        private void Start()
        {
            CurrentGameState = GameState.StartMenu;
            
            if (_isTest)
            {
                if (!_isWorldData)
                {
                    _sessionProgressService.SetWorldData(_world.WorldData); 
                }
                else
                {
                    _sessionProgressService.SetWorldData(_worldData);
                }
                
                _sessionProgressService.IndexCurrentLevel = _indexCurrentLevel;
                _dialogService.IsSkipDialogs = _isSkipDialogs;
                
                CurrentGameState = _startGameState;
            }
            
            OnUpdateGameState?.Invoke(_currentGameState);
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
                    _menuService.Close();
                    break;
                case GameState.StartMenu:
                    if (!_isTest)
                        yield return _loadScreenService.AwaitOpen();
                    
                    _startScreenService.Close();
                    break;
                case GameState.Shop:
                    if (!_isTest)
                        yield return _loadScreenService.AwaitOpen();
                    _shopService.Close();
                    _sessionProgressService.Gold = Constants.StartGold;
                    break;
                case GameState.Battle:
                    if (!_isTest)
                        yield return _loadScreenService.AwaitOpen();
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

            _backgroundService.BackgroundUpdate(_currentGameState);
            
            switch (_currentGameState)
            {
                case GameState.None:
                    break;
                case GameState.StartMenu:
                    _startScreenService.PreparationOpen();
                    _menuService.Close();
                    
                    if (_previousGameState != GameState.None)
                        yield return _loadScreenService.AwaitClose();

                    _startScreenService.Open();
                    break;
                case GameState.Shop:
                    if (_cutScenesService.GetCutscene())
                    {
                        if (!_isTest)
                            yield return _loadScreenService.AwaitClose();

                        print(_cutScenesService.GetCutscene());
                        yield return _cutScenesService.GetCutscene().Play();
                        _shopService.Open();
                    }
                    else
                    {
                        _shopService.Open();
                        
                        if (!_isTest)
                            yield return _loadScreenService.AwaitClose();
                    }

                    if (_sessionProgressService.IsEndData)
                    {
                        yield return AwaitDialogHide(_sessionProgressService.CurrentWorldData.DeathDialog);
                        _endWorldScreenService.Open();
                    }
                    
                    _menuService.Open();
                    break;
                case GameState.Battle:
                    _battleService.Open();
                    
                    if (!_isTest)
                        yield return _loadScreenService.AwaitClose();
                    _menuService.Open();
                    StartCoroutine(_battleService.AwaitProcessBattle());
                    break;
                case GameState.Titles:
                    _menuService.Close();
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
        
        private IEnumerator AwaitDialogHide(Dialogue dialogue, Action method = null)
        {
            _dialogService.Show(dialogue);
            var trigger = false;
            Action action = () => trigger = true;
            _dialogService.OnHide += action;
            yield return new WaitUntil(() => trigger);
            _dialogService.OnHide -= action;
            method?.Invoke();
        }
    }
}