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

        private void Start()
        {
            _button.onClick.AddListener(StartBattle);

            _loadScreenService.OnOpen += LoadedScreen;
            _loadScreenService.OnClose += () => _loadScreenService.StartCoroutine(OpenLoadScreen());
        }
        
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(StartBattle);
            
            _loadScreenService.OnOpen -= LoadedScreen;
            _loadScreenService.OnClose -= () => _loadScreenService.StartCoroutine(OpenLoadScreen());
        }

        private void StartBattle()
        {
            _loadScreenService.Close();
        }

        private IEnumerator OpenLoadScreen()
        {
            yield return new WaitForSeconds(0.2f);
            _loadScreenService.Open();
            _game.CurrentGameState = GameState.BattleTransition;
        }

        private void LoadedScreen()
        {
            _game.CurrentGameState = GameState.Battle;
        }
    }
}