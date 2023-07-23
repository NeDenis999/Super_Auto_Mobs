using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class StartBattleButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        
        private LoadScreenService _loadScreenService;
        private Game _game;

        [Inject]
        private void Construct(LoadScreenService loadScreenService, Game game)
        {
            _loadScreenService = loadScreenService;
            _game = game;
        }

        private void OnEnable()
        {
            //_game.OnUpdateGameState += CheckUpdateGameState;
        }

        private void OnDisable()
        {
            //_game.OnUpdateGameState -= CheckUpdateGameState;
        }

        private void Awake()
        {
            _button.onClick.AddListener(StartBattle);
            //CheckUpdateGameState(_game.CurrentGameState);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(StartBattle);
            
            _loadScreenService.OnOpen -= LoadedScreen;
            _loadScreenService.OnClose -= OpenLoadScreen;
        }

        private void CheckUpdateGameState(GameState gameState)
        {
            /*if (gameState == GameState.Shop)
            {
                _loadScreenService.OnClose += OpenLoadScreen;
            }*/
            /*else
            {
                _loadScreenService.OnOpen -= LoadedScreen;
                _loadScreenService.OnClose -= OpenLoadScreen;
            }*/
        }
        
        private void StartBattle()
        {
            _loadScreenService.OnClose += OpenLoadScreen;
            _loadScreenService.Close();
        }

        private void LoadedScreen()
        {
            _loadScreenService.OnOpen -= LoadedScreen;
            _game.CurrentGameState = GameState.Battle;
        }

        private void OpenLoadScreen()
        {
            _loadScreenService.OnOpen += LoadedScreen;
            _loadScreenService.OnClose -= OpenLoadScreen;
            _loadScreenService.StartCoroutine(AwaitOpenLoadScreen());
        }
        
        private IEnumerator AwaitOpenLoadScreen()
        {
            yield return new WaitForSeconds(0.2f);
            _loadScreenService.Open();
            _game.CurrentGameState = GameState.BattleTransition;
        }
    }
}