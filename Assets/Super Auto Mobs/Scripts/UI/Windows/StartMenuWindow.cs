using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class StartMenuWindow : BaseWindow
    {
        [SerializeField]
        private Button _storyButton;

        [SerializeField]
        private Button _settingsButton;

        [SerializeField]
        private Button _noAdsButton;
        
        [SerializeField]
        private Button _creatorsButton;
        
        [SerializeField]
        private Button _exitButton;

        private UIManager _uiManager;
        private BackgroundService _backgroundService;

        [Inject]
        private void Construct(UIManager uiManager, BackgroundService backgroundService)
        {
            _uiManager = uiManager;
            _backgroundService = backgroundService;
        }
        
        private void OnEnable()
        {
            _storyButton.onClick.AddListener(OnClickedStoryButton);
            _settingsButton.onClick.AddListener(OnClickedSettingsButton);
            _noAdsButton.onClick.AddListener(OnClickedNoAdsButton);
            _creatorsButton.onClick.AddListener(OnClickedCreatorsButton);
            _exitButton.onClick.AddListener(OnClickedExitButton);
        }
        
        private void OnDisable()
        {
            _storyButton.onClick.RemoveListener(OnClickedStoryButton);
            _settingsButton.onClick.RemoveListener(OnClickedSettingsButton);
            _noAdsButton.onClick.RemoveListener(OnClickedNoAdsButton);
            _creatorsButton.onClick.RemoveListener(OnClickedCreatorsButton);
            _exitButton.onClick.RemoveListener(OnClickedExitButton);
        }

        protected override void OnShow()
        {
            _backgroundService.BackgroundUpdate(GameState.StartMenu);
        }

        private void OnClickedStoryButton()
        {
            _uiManager.Show(WindowType.SelectWorld, null, false);
        }

        private void OnClickedSettingsButton()
        {
            _uiManager.Show(WindowType.Settings, null, false);
        }
        
        private void OnClickedNoAdsButton()
        {
            print("OnClickedNoAdsButton");
        }
        
        private void OnClickedCreatorsButton()
        {
            _uiManager.Show(WindowType.Titles);
        }
        
        private void OnClickedExitButton() { }
    }
}