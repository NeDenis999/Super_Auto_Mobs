using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class FlagButton : MonoBehaviour
    {
        public event Action<FlagButton> OnClick;
        
        [SerializeField]
        private LanguageService.Language _language;

        public LanguageService.Language Language => _language;

        private Button _button;
        private AssetProviderService _assetProviderService;
        private Image _image;

        [Inject]
        private void Construct(AssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
        }
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Click);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Click);
        }

        private void Click()
        {
            OnClick?.Invoke(this);
        }

        public void Off()
        {
            _image.material = _assetProviderService.DefaultMaterial;
        }

        public void On()
        {
            _image.material = _assetProviderService.OutlitMaterial;
        }
    }
}