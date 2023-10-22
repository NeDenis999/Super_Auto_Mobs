using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class PVPButton : MonoBehaviour
    {
        private Button _button;
        private LoaderLevelService _loaderLevelService;

        [Inject]
        private void Construct(LoaderLevelService loaderLevelService)
        {
            _loaderLevelService = loaderLevelService;
        }
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(LoadSceneGameplay);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(LoadSceneGameplay);
        }

        private void LoadSceneGameplay()
        {
            _loaderLevelService.LoadSceneGameplayPVP();
        }
    }
}