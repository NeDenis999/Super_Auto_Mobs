using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class CutScenesService : MonoBehaviour
    {
        [SerializeField]
        private List<CutScene> _cutScenes;

        [SerializeField]
        private bool _isTest;
        
        [SerializeField]
        private CutScene _testCutScene;

        private SessionProgressService _sessionProgressService;
        private Game _game;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService, Game game)
        {
            _sessionProgressService = sessionProgressService;
            _game = game;
        }
        
        private void Awake()
        {
            _game.OnUpdateGameState += CheckState;
        }

        private void CheckState(GameState gameState)
        {
            if (_isTest)
            {
                _testCutScene.Play();
                return;
            }
        }
    }

}