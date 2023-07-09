using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class DisableInteractiveShopOnActivate : MonoBehaviour
    {
        private Shop shopService;

        [Inject]
        private void Construct(Shop shopService)
        {
            this.shopService = shopService;
        }
        
        private void OnEnable()
        {
            shopService.IsInteractive = false;
        }

        private void OnDisable()
        {
            shopService.IsInteractive = true;
        }
    }
}