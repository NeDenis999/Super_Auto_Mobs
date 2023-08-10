using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class OpenStartScreen : MonoBehaviour
    {
        [SerializeField]
        private Screen _screen;
        
        private Game _game;
        private LoadScreenService _loadScreenService;

        [Inject]
        private void Construct(Game game, LoadScreenService loadScreenService)
        {
            _game = game;
            _loadScreenService = loadScreenService;
        }

        private void OnEnable()
        {
            _screen.OnFinalyClosing += OpenStartMenu;
        }

        private void OnDisable()
        {
            _screen.OnFinalyClosing -= OpenStartMenu;
        }

        private void OpenStartMenu()
        {
            //_loadScreenService.Close();
            _game.CurrentGameState = GameState.StartMenu;
        }
    }
}