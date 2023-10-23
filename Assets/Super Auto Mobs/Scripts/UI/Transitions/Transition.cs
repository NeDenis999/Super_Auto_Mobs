using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public abstract class Transition : MonoBehaviour
    {
        public abstract void OpenAnimate(BaseWindow window);

        public abstract UniTask CloseAnimate(BaseWindow window);
    }
}