using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class BaseWindow : MonoBehaviour
    {
        private Transition _transition;

        private void Awake()
        {
            _transition = GetComponentInChildren<Transition>();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            
            if (_transition)
            {
                _transition.OpenAnimate(this);
            }
            
            OnShow();
        }

        public virtual async UniTask Hide()
        {
            if (_transition)
                await _transition.CloseAnimate(this);
            
            OnHide();
            gameObject.SetActive(false);
        }

        public void Bind(object args)
        {
            if (args == null)
                return;

            OnBind(args);
        }

        protected virtual void OnBind(object args) { }

        protected virtual void OnShow() { }

        protected virtual void OnHide() { }
    }
}