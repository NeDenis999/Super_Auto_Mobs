using System.Collections.Generic;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class SkyMover : MonoBehaviour
    {
        private const float Size = 2f;
        private const float Speed = 0.5f;

        [SerializeField]
        private List<Transform> _clouds;

        private float _minX;
        private float _maxX;

        private void Start()
        {
            var mainCamera = Camera.main;
            _minX = mainCamera.ScreenToWorldPoint(Vector3.zero).x;
            _maxX = mainCamera.ScreenToWorldPoint(Vector3.zero.SetX(UnityEngine.Screen.width)).x;
            
            for (int i = 0; i < _clouds.Count; i++)
            {
                _clouds[i].position = _clouds[i].position.SetY(Mathf.Clamp(i, 3, 4.5f));
            }
            
            print("_minX" + _minX);
            print("_maxX" + _maxX);
        }

        private void Update()
        {
            for (int i = 0; i < _clouds.Count; i++)
            {
                _clouds[i].position = _clouds[i].position.SetX(Mathf.Repeat(Time.time * Speed - i * 5, 
                    _maxX - _minX + 2 * Size) + _minX - Size);
            }
        }
    }
}