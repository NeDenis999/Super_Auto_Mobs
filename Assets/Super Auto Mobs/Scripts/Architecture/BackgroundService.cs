using System;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class BackgroundService : MonoBehaviour
    {
        public Location Location => _location;
        
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private Location _defaultLocation;
        
        [SerializeField]
        private Location _startMenuLocation;
        
        private Location _location;
        private SessionProgressService _sessionProgressService;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService)
        {
            _sessionProgressService = sessionProgressService;
        }
        
        public void BackgroundUpdate(GameState gameState)
        {
            if (_location)
            {
                Destroy(_location.gameObject);
            }

            Location locationPrefab = null;
            
            switch (gameState)
            {
                case GameState.None:
                    throw new Exception("Недоступный стейт");
                case GameState.StartMenu:
                    locationPrefab = _startMenuLocation;
                    break;
                case GameState.Shop:
                    locationPrefab = _sessionProgressService.ShopLocation;
                    break;
                case GameState.Battle:
                    locationPrefab = _sessionProgressService.BattleLocation;
                    break;
                case GameState.Titles:
                    throw new Exception("Недоступный стейт");
                case GameState.World:
                    throw new Exception("Недоступный стейт");
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
            
            if (!locationPrefab)
                locationPrefab = _defaultLocation;
            
            _location = Instantiate(locationPrefab, transform);
            _camera.backgroundColor = _location.CameraColor;
        }
    }
}