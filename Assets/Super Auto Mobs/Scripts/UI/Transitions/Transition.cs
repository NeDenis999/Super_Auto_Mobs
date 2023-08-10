using System;
using System.Collections;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public abstract class Transition : MonoBehaviour
    {
        public abstract void OpenAnimate(Screen screen);

        public abstract void CloseAnimate(Screen screen, Action OnFinalyClosing);
    }
}