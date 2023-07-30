using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class MenuService : MonoBehaviour
    {
        [SerializeField]
        private OpenButton _openMenu, _openSetting;

        [SerializeField]
        private Game _game;

        private void OnEnable()
        {
            _game.OnUpdateGameState += UpdateGameState;
        }

        private void OnDisable()
        {
            _game.OnUpdateGameState -= UpdateGameState;
        }

        private void UpdateGameState(GameState gameState)
        {
            if (gameState != GameState.StartMenu)
            {
                _openMenu.enabled = true;
                _openSetting.enabled = false;
            }
            else
            {
                _openMenu.enabled = false;
                _openSetting.enabled = true;
            }
        }
    }
}