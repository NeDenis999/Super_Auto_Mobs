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

        [SerializeField]
        private List<Sprite> _dotsView;

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

            UpdateDotView();
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
            var trajectory = GetTrajectory(_startPoint, target);

            for (int i = 0; i < _dotsNumber; i++)
            {
                _trajectoryDots[i].transform.position = trajectory[i];
            }
        }

        public Vector2[] GetTrajectory(Vector2 startPoint, Vector2 target)
        {
            var points = new Vector2[_dotsNumber];
            var velocity = (target - startPoint) * _offsetMultiply * _offsetMultiply + _offset;
            
            float timeStep = _dotSpacing / velocity.magnitude;
            
            for (int i = 0; i < _dotsNumber; i++)
            {
                float t = i * timeStep;
                Vector2 point = startPoint + velocity * t + 0.5f * Physics2D.gravity * t * t;
                points[i] = point;
            }

            return points;
        }

        private void UpdateDotView()
        {
            for (int i = 0; i < _dotsNumber; i++)
            {
                if (i < _dotsView.Count)
                    _trajectoryDots[i].sprite = _dotsView[i];
                else
                    _trajectoryDots[i].sprite = _dotsView[^1];
            }
        }
    }
}