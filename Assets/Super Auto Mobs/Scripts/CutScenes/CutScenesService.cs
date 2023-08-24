using System;
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
        private CutScene _tutorial1;
        
        [SerializeField]
        private CutScene _tutorial2, _tutorial3;
        
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

        public CutScene GetCutscene()
        {
            if (!_sessionProgressService.IsCurrentWorld || _sessionProgressService.IsCutSceneComplete)
                return null;
            
            if (_isTest)
            {
                return _testCutScene;
            }

            switch (_sessionProgressService.CurrentWorld.WorldEnum)
            {
                case WorldEnum.Tutorial:
                    if (_sessionProgressService.CurrentWorld.IndexCurrentLevel == 0)
                    {
                        return _tutorial1;
                    }
                    
                    if (_sessionProgressService.CurrentWorld.IndexCurrentLevel == 1)
                    {
                        return _tutorial2;
                    }
                    
                    if (_sessionProgressService.CurrentWorld.IndexCurrentLevel == 2)
                    {
                        return _tutorial3;
                    }
                    break;
                case WorldEnum.DreamTour:
                    break;
                case WorldEnum.Mineshield:
                    if (_sessionProgressService.CurrentWorld.IndexCurrentLevel == 0)
                    {
                        return _mineShield1;
                    }
                    
                    if (_sessionProgressService.CurrentWorld.IndexCurrentLevel == 1)
                    {
                        return _mineShield2;
                    }
                    break;
                case WorldEnum.Grief:
                    break;
                case WorldEnum.Bav:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }
    }

}