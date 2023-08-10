using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using Zenject;

namespace Super_Auto_Mobs
{
    public class CutScene : MonoBehaviour
    {
        public UnityEvent OnPlay;
        public UnityEvent OnEnd;

        protected SessionProgressService _sessionProgressService;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService)
        {
            _sessionProgressService = sessionProgressService;
        }
        
        public virtual void Play()
        {
            OnPlay?.Invoke();
            OnEnd.AddListener(End);
        }

        private void End()
        {
            OnEnd.RemoveListener(End);
        }
    }
}