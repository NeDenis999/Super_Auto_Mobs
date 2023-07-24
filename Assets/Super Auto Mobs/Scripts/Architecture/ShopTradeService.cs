using System;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class ShopTradeService : MonoBehaviour
    {
        [SerializeField]
        private SessionProgressService _sessionProgress;

        private void Awake()
        {
            _sessionProgress.Gold = Constants.StartGold;
        }

        public void Sell()
        {
            _sessionProgress.Gold += Constants.PriceEntity;
        }

        public bool TryBuy(int cost = Constants.PriceEntity)
        {
            if (_sessionProgress.Gold >= cost)
            {
                _sessionProgress.Gold -= cost;
                return true;
            }
            
            return false;
        }
    }
}