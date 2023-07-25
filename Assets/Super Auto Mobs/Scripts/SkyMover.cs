using System.Collections.Generic;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class SkyMover : MonoBehaviour
    {
        private const float Size = 2f;
        private const float Speed = 0.5f;
        private const float MinY = 3;
        private const float MaxY = 5;

        [SerializeField]
        private Camera _camera;
        
        [SerializeField]
        private List<Transform> _clouds;

        [SerializeField]
        private Color _color;
        
        private float _minX;
        private float _maxX;

        private void Start()
        {
            if (_camera == null)
                _camera = Camera.main;

            for (int i = 0; i < _clouds.Count; i++)
            {
                _clouds[i].GetComponent<SpriteRenderer>().color = _color;
                _clouds[i].position = _clouds[i].position.SetY(transform.position.y + Random.Range(MinY, MaxY));
            }
        }

        private void Update()
        {
            _minX = _camera.ScreenToWorldPoint(Vector3.zero).x;
            _maxX = _camera.ScreenToWorldPoint(Vector3.zero.SetX(UnityEngine.Screen.width)).x;
            
            for (int i = 0; i < _clouds.Count; i++)
            {
                _clouds[i].position = _clouds[i].position.SetX(Mathf.Repeat(Time.time * Speed - i * 5, 
                    _maxX - _minX + 2 * Size) + _minX - Size);
            }
        }
    }
}