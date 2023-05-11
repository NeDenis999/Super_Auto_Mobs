using System.Collections.Generic;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class Trajectory : MonoBehaviour
    {
        [SerializeField]
        private GameObject _trajectoryDot; // Префаб отображения точек траектории
        
        [SerializeField]
        private int _dotsNumber; // Количество точек
        
        [SerializeField]
        private float _dotSpacing; // Расстояние между точками
        
        [SerializeField]
        private float _dotMinScale; // Минимальный размер точки
        
        [SerializeField]
        private float _dotMaxScale; // Максимальный размер точки
        
        [SerializeField]
        private Vector2 _offsetMultiply;
        
        [SerializeField]
        private Vector2 _offset;

        private Vector2 _startPoint;
        private List<SpriteRenderer> _trajectoryDots; // Список точек траектории
        private bool _select;

        void Start()
        {
            _trajectoryDots = new List<SpriteRenderer>();
            for (int i = 0; i < _dotsNumber; i++)
            {
                SpriteRenderer dot = Instantiate(_trajectoryDot, transform.position, Quaternion.identity, gameObject.transform).GetComponent<SpriteRenderer>();
                _trajectoryDots.Add(dot);
            }
        }

        void Update()
        {

        }

        public void Show(Vector2 startPoint)
        {
            _startPoint = startPoint.AddY(0.3f);
            gameObject.SetActive(true); // Включение отображения точек траектории
        }

        public void Hide()
        {
            gameObject.SetActive(false); // Выключение отображения точек траектории
        }

        public bool TrySelect()
        {
            if (_select)
                return false;
            
            foreach (var dot in _trajectoryDots)
            {
                dot.color = Color.black;
            }

            _select = true;
            return true;
        }

        public bool TryUnselect()
        {
            if (!_select)
                return false;
            
            foreach (var dot in _trajectoryDots)
            {
                dot.color = Color.white;
            }

            _select = false;
            return true;
        }

        public void Move(Vector2 target)
        {
            var velocity = (target - _startPoint) * _offsetMultiply * _offsetMultiply + _offset;
            
            float timeStep = _dotSpacing / velocity.magnitude; // Шаг времени
            for (int i = 0; i < _dotsNumber; i++)
            {
                float t = i * timeStep;
                Vector2 point = _startPoint + velocity * t + 0.5f * Physics2D.gravity * t * t; // Расчет точки траектории
                _trajectoryDots[i].transform.position = point;
                float scale = Mathf.Lerp(_dotMaxScale, _dotMinScale, t * 2);
                _trajectoryDots[i].transform.localScale = new Vector2(scale, scale);
            }
        }
    }
}