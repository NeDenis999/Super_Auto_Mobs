using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class CutScenesService : MonoBehaviour
    {
        [SerializeField]
        private ProgressEnum _progressEnum;
        
        [SerializeField]
        private List<CutScene> _cutScenes;

        [SerializeField]
        private bool _isTest;
        
        [SerializeField]
        private CutScene _testCutScene;

        private SessionProgressService _sessionProgressService;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService)
        {
            _sessionProgressService = sessionProgressService;
        }
        
        private void Start()
        {
            if (_isTest)
            {
                _testCutScene.Play();
                return;
            }
            
            if (_sessionProgressService.IsTest)
                _cutScenes[0].Play();
        }
    }

}