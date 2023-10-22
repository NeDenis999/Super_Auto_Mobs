using System;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Entity : MonoBehaviour
    {
        public event Action OnSelected;
        public event Action OnUnselected;
        
        protected Title _name = new() {English = "Name", Russian = "Имя"};
        protected Title _info = new() { English = "Info", Russian = "Описание" };
        protected SpriteRenderer _spriteRenderer;
        protected AssetProviderService _assetProviderService;
        
        private Material _defaultMaterial, _outlitMaterial;
        private LTSeq _sequence;
        protected SoundsService _soundsService;

        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Title Info => _info;
        public Title Name => _name;

        [Inject]
        private void Construct(AssetProviderService assetProviderService, SoundsService soundsService)
        {
            _soundsService = soundsService;
            _assetProviderService = assetProviderService;

            _defaultMaterial = _assetProviderService.DefaultMaterial;
            _outlitMaterial = _assetProviderService.OutlitMaterial;
        }

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _sequence = LeanTween.sequence();
        }

        public void Select()
        {
            _spriteRenderer.material = _outlitMaterial;
            _spriteRenderer.sortingLayerName = "UI";
            
            OnSelected?.Invoke();
        }
        
        public void Unselect()
        {
            _spriteRenderer.material = _defaultMaterial;
            _spriteRenderer.sortingLayerName = "Default";

            OnUnselected?.Invoke();
        }

        public void UpScaleSmoothly()
        {
            _soundsService.PlayClick();

            LeanTween.scale(gameObject, Vector3.one * 1.1f, 0.2f)
                .setEaseOutBack()
                .setOnComplete(() => LeanTween.scale(gameObject, Vector3.one * 1.02f, 0.2f).setEaseOutBack());
        }

        public void DownScaleSmoothly()
        {
            _soundsService.PlayClick();
            
            LeanTween.scale(gameObject, Vector3.one, 0.2f)
                .setEaseOutBack();
        }
    }
}