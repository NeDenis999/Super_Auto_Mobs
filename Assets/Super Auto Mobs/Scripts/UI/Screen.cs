using System;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class Screen : MonoBehaviour
    {
        public event Action OnBeginOpen;
        public event Action OnFinalyClosing;

        private Transition _transition;
        private bool _firstOpen;
        
        public virtual void Open()
        {
            OnBeginOpen?.Invoke();
            gameObject.SetActive(true);
            
            if (!_transition && !_firstOpen)
            {
                _transition = GetComponent<Transition>();
                _firstOpen = true;
            }
            
            if (_transition)
            {
                _transition.OpenAnimate(this);
            }
        }

        public virtual void Close()
        {
            if (_transition)
                _transition.CloseAnimate(this, OnFinalyClosing);
            else
            {
                OnFinalyClosing?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}