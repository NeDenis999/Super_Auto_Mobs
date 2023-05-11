using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class Titles : Screen
    {
        //08.05.2023

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private float _tiemMove;
        
        [SerializeField]
        private float _startDelay;

        [SerializeField]
        private float _widhtSize;

        private void OnEnable()
        {
            StartCoroutine(TextMove());
        }
        
        private void OnDisable()
        {
            StopCoroutine(TextMove());
        }

        private IEnumerator TextMove()
        {
            _text.transform.position = _text.transform.position.SetY(-UnityEngine.Screen.height / 2 - _widhtSize);
            yield return new WaitForSeconds(_startDelay);
            yield return LeanTween.moveY(_text.gameObject, UnityEngine.Screen.height + _widhtSize, _tiemMove);
        }
    }
}