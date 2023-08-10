using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class StoryButton : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _mainMenu;
        
        private Button _button;
        private LoaderLevelService _loaderLevelService;
        private LoadScreenService _loadScreenService;

        [Inject]
        private void Construct(LoaderLevelService loaderLevelService, LoadScreenService loadScreenService)
        {
            _loaderLevelService = loaderLevelService;
            _loadScreenService = loadScreenService;
        }
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OpenLoadScreen);
            _loadScreenService.OnOpen += LoadSceneGameplay;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OpenLoadScreen);
            _loadScreenService.OnOpen -= LoadSceneGameplay;
        }

        private void OpenLoadScreen()
        {
            _mainMenu.LeanAlpha(0 , 0.25f);
            _loadScreenService.Open();
        }
        
        private void LoadSceneGameplay()
        {
            _loaderLevelService.LoadSceneGameplayStory();
            _loadScreenService.Close();
        }
    }
}