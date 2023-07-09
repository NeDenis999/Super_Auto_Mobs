using System;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class LevelDisplay : MonoBehaviour
    {
        private Game _game;
        private CanvasGroup _canvasGroup;
        
        [Inject]
        private void Construct(Game game)
        {
            _game = game;
        }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            CheckGameState(_game.CurrentGameState);
            
            _game.OnUpdateGameState += CheckGameState;
        }

        private void OnDestroy()
        {
            _game.OnUpdateGameState -= CheckGameState;
        }

        private void CheckGameState(GameState gameState)
        {
            if (gameState == GameState.BattleTransition)
            {
                Close();
            }
            else if (gameState == GameState.Shop)
            {
                Open();
            }
        }

        private void Open()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        private void Close()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}