using System;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Entity : MonoBehaviour
    {
        public event Action OnSelected;
        public event Action OnUnselected;
        
        private Material _defaultMaterial, _outlitMaterial;
        private SpriteRenderer _spriteRenderer;
        private AssetProviderService _assetProviderService;

        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        [Inject]
        private void Construct(AssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
        }
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _defaultMaterial = _assetProviderService.DefaultMaterial;
            _outlitMaterial = _assetProviderService.OutlitMaterial;
        }
        
        public void Select()
        {
            _spriteRenderer.material = _outlitMaterial;
            _spriteRenderer.sortingOrder = 5;
            
            OnSelected?.Invoke();
        }
        
        public void Unselect()
        {
            _spriteRenderer.material = _defaultMaterial;
            _spriteRenderer.sortingOrder = 4;
            
            OnUnselected?.Invoke();
        }
    }
}