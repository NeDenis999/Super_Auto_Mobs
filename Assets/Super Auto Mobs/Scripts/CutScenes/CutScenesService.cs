using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class CutScenesService : MonoBehaviour
    {
        [SerializeField]
        private bool _isSkip;
        
        [SerializeField]
        private bool _isTest;
        
        [SerializeField]
        private CutScene _testCutScene;

        [Header("Cut Scenes")]
        [SerializeField]
        private CutScene _tutorial;
        
        [SerializeField]
        private CutScene _mineShield1, _mineShield2;
        
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
            if (!_sessionProgressService.IsCurrentWorld)
                return;
            
            if (_isTest)
            {
                _testCutScene.Play();
                return;
            }

            switch (_sessionProgressService.CurrentWorld.WorldEnum)
            {
                case WorldEnum.Tutorial:
                    if (gameState == GameState.Shop)
                    {
                        if (_sessionProgressService.CurrentWorld.IndexCurrentLevel == 0)
                        {
                            _tutorial.Play();
                        }
                    }
                    break;
                case WorldEnum.DreamTour:
                    break;
                case WorldEnum.Mineshield:
                    if (gameState == GameState.Shop)
                    {
                        if (_sessionProgressService.CurrentWorld.IndexCurrentLevel == 0)
                        {
                            _mineShield1.Play();
                        }
                        else if (_sessionProgressService.CurrentWorld.IndexCurrentLevel == 1)
                        {
                            _mineShield2.Play();
                        }
                    }
                    break;
                case WorldEnum.Grief:
                    break;
                case WorldEnum.Bav:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

}