using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Game : MonoBehaviour
    {
        public event Action<GameState> OnUpdateGameState;
        
        public GameState CurrentGameState
        {
            get => _currentGameState;
            set
            {   
                _currentGameState = value;
                OnUpdateGameState?.Invoke(_currentGameState);

                switch (_currentGameState)
                {
                    case GameState.StartMenu:
                        break;
                    case GameState.ShopTransition:
                        break;
                    case GameState.Shop:
                        _shopService.Open();
                        _battleService.Close();
                        break;
                    case GameState.BattleTransition:
                        _battleService.Open();
                        _shopService.Close();
                        break;
                    case GameState.Battle:
                        StartCoroutine(_battleService.AwaitProcessBattle());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        [SerializeField]
        private GameState _currentGameState;

        private Shop _shopService;
        private BattleService _battleService;

        [Inject]
        private void Construct(Shop shopService, BattleService battleBaseService)
        {
            _shopService = shopService;
            _battleService = battleBaseService;
        }
        
        private void Start()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
                CurrentGameState = GameState.StartMenu;
            else
                CurrentGameState = GameState.Shop;
        }
    }
}