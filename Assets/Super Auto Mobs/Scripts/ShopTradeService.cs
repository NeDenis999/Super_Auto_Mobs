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
            _sessionProgress.Emeralds = 10;
        }

        public void Sell()
        {
            _sessionProgress.Emeralds += 3;
        }

        public bool TryBuy(int cost = 3)
        {
            if (_sessionProgress.Emeralds >= cost)
            {
                _sessionProgress.Emeralds -= cost;
                return true;
            }
            
            return false;
        }
    }
}