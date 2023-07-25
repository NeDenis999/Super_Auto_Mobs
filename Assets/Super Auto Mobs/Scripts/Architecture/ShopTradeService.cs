using System;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class ShopTradeService : MonoBehaviour
    {
        private SessionProgressService _sessionProgressService;
        private SoundsService _soundsService;

        [Inject]
        private void Construct(SoundsService soundsService, SessionProgressService sessionProgressService)
        {
            _soundsService = soundsService;
            _sessionProgressService = sessionProgressService;
        }
        
        private void Awake()
        {
            _sessionProgressService.Gold = Constants.StartGold;
        }

        public void Sell()
        {
            _soundsService.PlayBuy();
            _sessionProgressService.Gold += Constants.PriceEntity;
        }

        public bool TryBuy(int cost = Constants.PriceEntity)
        {
            if (_sessionProgressService.Gold >= cost)
            {
                _soundsService.PlayBuy();
                _sessionProgressService.Gold -= cost;
                return true;
            }
            
            return false;
        }
    }
}