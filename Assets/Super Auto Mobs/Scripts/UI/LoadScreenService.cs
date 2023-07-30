using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Super_Auto_Mobs
{
    public class LoadScreenService : MonoBehaviour
    {
        public event Action OnOpen;
        public event Action OnClose;
        
        [SerializeField]
        private Material _material;

        [SerializeField]
        private string _parametr = "_ProgressAmount";

        [SerializeField]
        private float _speed = 1;

        [SerializeField]
        private Image _image;
        
        [SerializeField]
        private SessionProgressService _sessionProgressService;
        
        [SerializeField]
        private AssetProviderService _assetProviderService;

        [SerializeField]
        private List<Texture> _textures;
         
        private bool _isActive;
        private bool _isOpen = true;
        private float _progress = 1;

        private void Start()
        {
            _material.SetFloat(_parametr, 1);
            _isActive = true;
            _progress = 1;
        }

        private void Update()
        {
            _progress = Mathf.Clamp(_progress, 0, 1);
            _material.SetFloat(_parametr, _progress);
            
            if (_isActive)
            {
                _progress += Time.deltaTime * _speed * (1.05f - _progress);
                
                if (!_isOpen && _progress >= 1f)
                {
                    _isOpen = true;
                    OnOpen?.Invoke();
                    print("Open");
                }
            }
            else
            {
                _progress -= Time.deltaTime * _speed * (1.005f - _progress) * 2;
                
                if (_isOpen && _progress <= 0f)
                {
                    _isOpen = false;
                    OnClose?.Invoke();
                    print("Close");
                }
            }
        }

        [ContextMenu("Open")]
        public void Open()
        {
            UpdateTexture();
            
            _progress = 0;
            _isActive = true;
            _material.SetFloat(_parametr, 0);
            _isOpen = false;
        }
        
        [ContextMenu("Close")]
        public void Close()
        {
            UpdateTexture();
            /*if (_sessionProgressService.MobsUnlocked.Count > 0)
            {
                _image.sprite = _assetProviderService
                    .GetMobInfo(_sessionProgressService.MobsUnlocked[Random.Range(0, _sessionProgressService.MobsUnlocked.Count)])
                    .Prefab.GetComponent<SpriteRenderer>()
                    .sprite;
            }
            else
            {
                _image.sprite = _defaultImage;
               // _material.SetTexture("_TransitionTex", _defaultImage.texture);
            }*/
            
            _progress = 1;
            _isActive = false;
            _material.SetFloat(_parametr, 1);
            _isOpen = true;
        }

        private void UpdateTexture()
        {
            _material.SetTexture("_TransitionTex", _textures[Random.Range(0, _textures.Count)]);
        }
    }
}