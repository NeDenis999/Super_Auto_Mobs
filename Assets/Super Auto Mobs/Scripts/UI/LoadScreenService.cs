using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Super_Auto_Mobs
{
    public class LoadScreenService : MonoBehaviour
    {
        public event Action OnClose;
        public event Action OnOpen;
        
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
        private MenuService _menuService;
        
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
            _material.SetFloat(_parametr, _progress);
            
            if (_isActive)
            {
                _progress += Time.deltaTime * _speed * (1.05f - _progress);
                
                if (!_isOpen && _progress >= 1f)
                {
                    _isOpen = true;
                    print("Close");
                }
            }
            else
            {
                _progress -= Time.deltaTime * _speed * (1.005f - _progress) * 2;
                
                if (_isOpen && _progress <= 0f)
                {
                    _isOpen = false;
                    print("Open");
                }
            }
            
            _progress = Mathf.Clamp(_progress, 0, 1);
        }

        [ContextMenu("Close")]
        public IEnumerator Close()
        {
            UpdateTexture();
            
            _progress = 0;
            _isActive = true;
            _material.SetFloat(_parametr, 0);
            _isOpen = false;
            
            yield return new WaitUntil(() => _isOpen);
            _menuService.Menu.SetActive(true);
            OnClose?.Invoke();
        }
        
        [ContextMenu("Open")]
        public IEnumerator Open()
        {
            UpdateTexture();
            
            _menuService.Menu.SetActive(false);
            
            _progress = 1;
            _isActive = false;
            _material.SetFloat(_parametr, 1);
            _isOpen = true;
            
            yield return new WaitUntil(() => !_isOpen);
            OnOpen?.Invoke();
        }

        private void UpdateTexture()
        {
            _material.SetTexture("_TransitionTex", _textures[Random.Range(0, _textures.Count)]);
        }

        private void OnDestroy()
        {
            _material.SetFloat(_parametr, 1);
        }
    }
}