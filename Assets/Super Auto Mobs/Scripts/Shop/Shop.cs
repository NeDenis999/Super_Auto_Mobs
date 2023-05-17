using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Shop : MonoBehaviour
    {
        private ShopPlatformService _shopPlatformService;

        [Inject]
        private void Construct(ShopPlatformService shopPlatformService)
        {
            _shopPlatformService = shopPlatformService;
        }
        
        private void Update()
        {
            _shopPlatformService.PlatformServiceUpdate();
            _shopPlatformService.PlatformsPositionUpdate();
        }
    }
}