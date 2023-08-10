using System;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class ShopTradeService : MonoBehaviour
    {
        public event Action<PurchaseEnum> OnBuy;
        
        private SessionProgressService _sessionProgressService;
        private SoundsService _soundsService;

        [Inject]
        private void Construct(SoundsService soundsService, SessionProgressService sessionProgressService)
        {
            _soundsService = soundsService;
            _sessionProgressService = sessionProgressService;
        }

        public void Sell()
        {
            _soundsService.PlayBuy();
            _sessionProgressService.Gold += Constants.PriceEntity;
        }

        public bool TryBuy(PurchaseEnum purchaseEnum, int cost = Constants.PriceEntity)
        {
            if (_sessionProgressService.Gold >= cost)
            {
                _soundsService.PlayBuy();
                _sessionProgressService.Gold -= cost;
                OnBuy?.Invoke(purchaseEnum);
                return true;
            }
            
            return false;
        }
    }
}