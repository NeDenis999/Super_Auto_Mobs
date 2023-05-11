using System;
using System.Collections;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class ScaleTransition : Transition
    {
        public override void OpenAnimate(Screen screen)
        {
            transform.localScale = Vector3.one / 2;
            LeanTween.scale(gameObject, Vector3.one, 0.2f);
        }

        public override void CloseAnimate(Screen screen, Action OnOnStartedClosing)
        {
            transform.localScale = Vector3.one;
            LeanTween
                .scale(gameObject, Vector3.one / 2, 0.2f)
                .setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                    OnOnStartedClosing?.Invoke();
                });
        }
    }
}