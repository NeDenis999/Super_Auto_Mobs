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
        
        private LTSeq _sequence;

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
            //LeanTween.cancel(gameObject);
            /*_sequence.append(*/
                LeanTween.scale(gameObject, Vector3.one * 1.1f, 0.2f)
                    .setEaseOutBack()
                    .setOnComplete(() => LeanTween.scale(gameObject, Vector3.one * 1.02f, 0.2f).setEaseOutBack());
        }

        public void DownScaleSmoothly()
        {
            //LeanTween.cancel(gameObject);
            /*_sequence.append(*/
                LeanTween.scale(gameObject, Vector3.one, 0.2f)
                    .setEaseOutBack();
        }

        public void MoveSmoothly(float target)
        {
            LeanTween.cancel(gameObject);
            /*_sequence.append(*/
                LeanTween.moveX(gameObject, target, 0.5f)
                    .setEaseOutBack();
        }
    }
}