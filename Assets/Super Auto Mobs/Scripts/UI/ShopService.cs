using UnityEngine;

namespace Super_Auto_Mobs
{
    public class ShopService : MonoBehaviour
    {
        [SerializeField]
        private SessionProgressService _sessionProgress;
        
        public void Sell()
        {
            _sessionProgress.Emeralds += 3;
        }
    }
}