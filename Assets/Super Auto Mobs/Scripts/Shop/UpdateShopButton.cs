using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class UpdateShopButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        private ShopUpdaterService _shopUpdaterService;
        private ShopTradeService shopTradeService;

        [Inject]
        private void Construct(ShopUpdaterService shopUpdaterService, ShopTradeService shopTradeService)
        {
            _shopUpdaterService = shopUpdaterService;
            this.shopTradeService = shopTradeService;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(UpdateShop);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(UpdateShop);
        }

        private void UpdateShop()
        {
            if (shopTradeService.TryBuy(1))
            {
                _shopUpdaterService.UpdateShop();
            }
        }
    }
}