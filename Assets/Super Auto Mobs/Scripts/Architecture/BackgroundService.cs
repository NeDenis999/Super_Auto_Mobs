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

            if (gameState != GameState.Shop && gameState != GameState.Battle)
                return;

            var locationPrefab = gameState == GameState.Shop
                ? _sessionProgressService.ShopLocation
                : _sessionProgressService.BattleLocation;
            
            if (!locationPrefab)
                locationPrefab = _defaultLocation;
            
            _location = Instantiate(locationPrefab, transform);
            _camera.backgroundColor = _location.CameraColor;
        }
    }
}