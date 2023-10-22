using UnityEngine;
using UnityEngine.Serialization;

namespace Super_Auto_Mobs
{
    public class StartScreenService : MonoBehaviour
    {
        [SerializeField]
        private GameObject _menu;

        [SerializeField]
        private Camera _myCamera;
        
        [SerializeField]
        private Camera _menuCamera;

        [SerializeField]
        private GameObject _blurCanvas;
        
        [SerializeField]
        private GameObject _canvas;
        
        [SerializeField]
        private Screen _selectWorldScreen, _firstOpenGameScreen;
        
        [SerializeField]
        private Screen _blackoutScreen;

        [SerializeField]
        private SessionProgressService _sessionProgressService;

        [SerializeField]
        private SoundsService _soundsService;
        
        [SerializeField]
        private Game _game;

        private World _world;
        [FormerlySerializedAs("WarningScreen")] public WarningWindow warningWindow;

        public void PreparationOpen()
        {
            _menu.SetActive(true);
            _myCamera.gameObject.SetActive(false);
            _menuCamera.gameObject.SetActive(true);
        }
        
        public void Open()
        {
            _blurCanvas.SetActive(true);
            _canvas.SetActive(true);
            _soundsService.PlayMenu();

            if (!_sessionProgressService.IsNotFirsOpenGame && !_game.IsTest)
            {
                _firstOpenGameScreen.Open();
                _sessionProgressService.IsNotFirsOpenGame = true;
            }
        }
        
        public void Close()
        {
            if (!_menu.activeSelf)
                return;

            _menu.SetActive(false);
            _myCamera.gameObject.SetActive(true);
            _menuCamera.gameObject.SetActive(false);
            _blurCanvas.SetActive(false);
        }

        public void OpenWorld(World world)
        {
            _world = world;
            
            _selectWorldScreen.OnFinalyClosing += CloseSelectWorldScreen;
            _selectWorldScreen.Close();
            _blackoutScreen.Close();
            _sessionProgressService.SetWorldData(_world.WorldData);
        }

        private void CloseSelectWorldScreen()
        {
            _selectWorldScreen.OnFinalyClosing -= CloseSelectWorldScreen;
            _canvas.SetActive(false);
            _game.CurrentGameState = GameState.Shop;
        }
    }
}