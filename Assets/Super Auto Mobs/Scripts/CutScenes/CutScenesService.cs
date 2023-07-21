using System;
using System.Collections.Generic;
using UnityEngine;

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
        
        private void Start()
        {
            if (_isTest)
                _testCutScene.Play();
        }
    }

}