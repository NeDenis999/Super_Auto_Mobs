
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Super_Auto_Mobs
{
    public class ConnectScreen : MonoBehaviour
    {
        [SerializeField]
        private Transform _rotatedTransform;

        [SerializeField]
        private float _rotatedSpeed, _speedLoad, _delayTextUpdate;

        [SerializeField]
        private Slider _loadSlider;

        [SerializeField]
        private TextMeshProUGUI _loadText;
        
        private bool _isLoad => _loadSlider.value < _loadSlider.maxValue;

        private void Start()
        {
            _loadSlider.value = 0;
        }

        private void Update()
        {
            if (_loadSlider.value < _loadSlider.maxValue)
                _loadSlider.value += Time.deltaTime * _speedLoad;
            
            _rotatedTransform.Rotate(Vector3.forward, Time.deltaTime * _rotatedSpeed);
        }
    }
}