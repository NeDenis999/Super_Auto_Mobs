using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class OpenStartScreen : MonoBehaviour
    {
        private Game _game;
        private Button _button;

        [Inject]
        private void Construct(Game game)
        {
            _game = game;
        }
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OpenStartMenu);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OpenStartMenu);
        }

        private void OpenStartMenu()
        {
            _game.CurrentGameState = GameState.StartMenu;
        }
    }
}