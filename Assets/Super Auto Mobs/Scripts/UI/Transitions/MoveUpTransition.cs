using System;
using Cysharp.Threading.Tasks;
using ModestTree;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class MoveUpTransition : Transition
    {
        [SerializeField]
        private float _sizeY;

        private float _centerPosition => UnityEngine.Screen.height / 2;
        private float _upPosition => UnityEngine.Screen.height + _sizeY / 2;
        
        public override void OpenAnimate(BaseWindow screen)
        {
            transform.position = transform.position.SetY(_upPosition);
            LeanTween.moveY(gameObject, _centerPosition, 0.2f);
        }

        public override async UniTask CloseAnimate(BaseWindow screen)
        {
            transform.position = transform.position.SetY(_centerPosition);
            LeanTween
                .moveY(gameObject, _upPosition, 0.2f);

            await UniTask.Delay(200);
        }
    }
}