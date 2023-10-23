using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class MenuWindow : BaseWindow
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private Button _settingsButton;
        
        [SerializeField]
        private Button _exitToStartMenuButton;

        private Game _game;
        private UIManager _uiManager;

        [Inject]
        private void Construct(Game game, UIManager uiManager)
        {
            _game = game;
            _uiManager = uiManager;
        }
        
        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnClickedCloseButton);
            _settingsButton.onClick.AddListener(OnClickedSettingsButton);
            _exitToStartMenuButton.onClick.AddListener(OnClickedExitToStartMenuButton);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnClickedCloseButton);
            _settingsButton.onClick.RemoveListener(OnClickedSettingsButton);
            _exitToStartMenuButton.onClick.RemoveListener(OnClickedExitToStartMenuButton);
        }
        
        private void OnClickedCloseButton()
        {
            Hide();
        }
        
        private void OnClickedSettingsButton()
        {
            _uiManager.Show(WindowType.Settings);
        }
        
        private void OnClickedExitToStartMenuButton()
        {
            _game.CurrentGameState = GameState.StartMenu;
        }
    }
}