using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Super_Auto_Mobs
{
    public class CutScene : MonoBehaviour
    {
        public UnityEvent OnPlay;
        public UnityEvent OnEnd;

        protected SessionProgressService _sessionProgressService;
        protected DialogService _dialogService;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService, DialogService dialogService)
        {
            _dialogService = dialogService;
            _sessionProgressService = sessionProgressService;
        }
        
        public virtual IEnumerator Play()
        {
            _sessionProgressService.IsCutSceneComplete = true;
            OnPlay?.Invoke();
            OnEnd.AddListener(End);
            yield return null;
        }

        private void End()
        {
            OnEnd.RemoveListener(End);
        }
        
        protected IEnumerator AwaitDialogHide(Dialogue dialogue, Action method = null)
        {
            _dialogService.Show(dialogue);
            var trigger = false;
            Action action = () => trigger = true;
            _dialogService.OnHide += action;
            yield return new WaitUntil(() => trigger);
            _dialogService.OnHide -= action;
            method?.Invoke();
        }
    }
}