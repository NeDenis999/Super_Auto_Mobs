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
        
        private Game _game;

        [Inject]
        private void Construct(Game game)
        {
            _game = game;
        }

        private void Awake()
        {
            _button.onClick.AddListener(StartBattle);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(StartBattle);
        }

        private void StartBattle()
        {
            _game.CurrentGameState = GameState.Battle;
        }
    }
}