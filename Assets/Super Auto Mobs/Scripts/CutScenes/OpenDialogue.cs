﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Super_Auto_Mobs
{
    public class OpenDialogue : MonoBehaviour
    {
        private DialogService _dialogService;

        [SerializeField]
        private UnityEvent _onEndDialogue;

        [SerializeField]
        private Dialogue _dialogue;
        
        [Inject]
        private void Construct(DialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public void Open()
        {
            _dialogService.Show(_dialogue);
            _dialogService.OnHide += _onEndDialogue.Invoke;
        }
    }
}