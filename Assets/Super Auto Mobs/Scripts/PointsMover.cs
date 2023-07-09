using System;
using System.Collections.Generic;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class PointsMover : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> _points;

        [SerializeField]
        private float _speed;
        
        private int _currentPointNumber;
        
        private void Update()
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _points[_currentPointNumber].position, 
                        _speed * Time.deltaTime).SetZ(-10);

            if (((Vector2)transform.position - (Vector2)_points[_currentPointNumber].position).magnitude < 0.5f)
            {
                _currentPointNumber++;

                if (_currentPointNumber >= _points.Count)
                {
                    _currentPointNumber = 0;
                }
            }
        }
    }
}