using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Super_Auto_Mobs
{
    public class CutScene : MonoBehaviour
    {
        public string Name;
        public UnityEvent OnPlay;
        public UnityEvent OnEnd;

        public virtual void Play()
        {
            OnPlay?.Invoke();
        }
    }
}