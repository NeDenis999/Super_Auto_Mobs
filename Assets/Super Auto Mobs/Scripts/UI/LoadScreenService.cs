using System;
using UnityEngine;

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
            _progress = 0;
            _isActive = true;
            _material.SetFloat(_parametr, 0);
            _isOpen = false;
        }
        
        [ContextMenu("Close")]
        public void Close()
        {
            _progress = 1;
            _isActive = false;
            _material.SetFloat(_parametr, 1);
            _isOpen = true;
        }
    }
}