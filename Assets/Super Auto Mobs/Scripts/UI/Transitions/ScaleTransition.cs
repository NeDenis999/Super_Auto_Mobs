using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class ScaleTransition : Transition
    {
        public override void OpenAnimate(BaseWindow screen)
        {
            transform.localScale = Vector3.one / 2;
            LeanTween.scale(gameObject, Vector3.one, 0.2f);
        }

        public override async UniTask CloseAnimate(BaseWindow screen)
        {
            transform.localScale = Vector3.one;
            LeanTween
                .scale(gameObject, Vector3.one / 2, 0.2f);
            
            await UniTask.Delay(200);
        }
    }
}