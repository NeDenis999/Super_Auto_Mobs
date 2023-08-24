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
        public event Action OnStartOpen;

        [SerializeField]
        private AnimationCurve _openCurve, _closeCurve;
        
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
        
        private float _progress = 1;
        private AnimationCurve _currentCurve;

        private void Start()
        {
            _material.SetFloat(_parametr, 1);
        }

        private void Update()
        {
            if (_progress >= 1)
                return;
            
            _progress += Time.deltaTime * _speed;
            _progress = Mathf.Clamp(_progress, 0, 1);
            _material.SetFloat(_parametr, Mathf.Clamp(_currentCurve.Evaluate(_progress), 0, 1));
        }
        
        public IEnumerator AwaitClose()
        {
            UpdateTexture();
            
            _progress = 0;
            _material.SetFloat(_parametr, 0);
            _currentCurve = _closeCurve;
            
            yield return new WaitUntil(() =>  Math.Abs(_progress - 1) < 0.000001f);
            _menuService.Menu.SetActive(true);
            OnClose?.Invoke();
        }
        
        public IEnumerator AwaitOpen()
        {
            OnStartOpen?.Invoke();
            UpdateTexture();
            
            _menuService.Menu.SetActive(false);
            
            _progress = 0;
            _material.SetFloat(_parametr, 1);
            _currentCurve = _openCurve;
            
            yield return new WaitUntil(() => Math.Abs(_progress - 1) < 0.000001f);
            OnOpen?.Invoke();
        }

        [ContextMenu("Open")]
        public void Open()
        {
            StartCoroutine(AwaitOpen());
        }
        
        [ContextMenu("Close")]
        public void Close()
        {
            StartCoroutine(AwaitClose());
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