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
        private SessionProgressService _sessionProgressService;
        private LoadScreenService _loadScreenService;

        private bool _isLoad;
        private BackgroundService _backgroundService;
        private CutScenesService _cutScenesService;
        private UIManager _uiManager;

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
        private void Construct(ShopService shopService, BattleService battleBaseService,
            SessionProgressService sessionProgressService, LoadScreenService loadScreenService,
            BackgroundService backgroundService, CutScenesService cutScenesService, UIManager uiManager)
        {
            _cutScenesService = cutScenesService;
            _backgroundService = backgroundService;
            _shopService = shopService;
            _battleService = battleBaseService;
            _sessionProgressService = sessionProgressService;
            _loadScreenService = loadScreenService;
            _uiManager = uiManager;
        }

        private void Start()
        {
            _uiManager.HideAll();
            
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
                //_dialogService.IsSkipDialogs = _isSkipDialogs;
                
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
                    _shopService.Close();
                    _battleService.Close();
                    break;
                case GameState.StartMenu:
                    if (!_isTest)
                        yield return _loadScreenService.AwaitOpen();
                    
                    //_startScreenService.Close();
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
                    //_titlesService.Close();
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
                    _uiManager.Show(WindowType.StartMenu);

                    if (_previousGameState != GameState.None)
                        yield return _loadScreenService.AwaitClose();
                    
                    break;
                case GameState.Shop:
                    if (_cutScenesService.GetCutscene())
                    {
                        if (!_isTest)
                            yield return _loadScreenService.AwaitClose();
                        
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
                        //yield return AwaitDialogHide(_sessionProgressService.CurrentWorldData.DeathDialog);
                        _uiManager.Show(WindowType.WorldEnd);
                    }
                    
                    _uiManager.Show(WindowType.Menu);
                    break;
                case GameState.Battle:
                    _battleService.Open();
                    
                    if (!_isTest)
                        yield return _loadScreenService.AwaitClose();
                    _uiManager.Show(WindowType.Menu);
                    StartCoroutine(_battleService.AwaitProcessBattle());
                    break;
                case GameState.Titles:
                    _uiManager.Show(WindowType.Titles);
                    //_titlesService.Open();
                    break;
                case GameState.World:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _isLoad = false;
            OnUpdateGameState?.Invoke(_currentGameState);
        }

        public void OpenWorld(World world)
        {
            _world = world;
            
            /*_selectWorldScreen.OnFinalyClosing += CloseSelectWorldScreen;
            _selectWorldScreen.Close();
            _blackoutScreen.Close();*/
            _uiManager.HideAll();
            _sessionProgressService.SetWorldData(_world.WorldData);
            StartCoroutine(AwaitCurrentGameState(GameState.Shop));
        }

        private void CloseSelectWorldScreen()
        {
            //_selectWorldScreen.OnFinalyClosing -= CloseSelectWorldScreen;
            //_canvas.SetActive(false);
            //_game.CurrentGameState = GameState.Shop;
        }
    }
}