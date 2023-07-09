using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class AddCoinsPerk : Perk
    {
        [SerializeField]
        private int _countCoins = 1;
        
        private SessionProgressService _sessionProgressService;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService)
        {
            _sessionProgressService = sessionProgressService;
        }
        
        public override void Activate()
        {
            print("AddCoinsPerk Activate");
            AddCoins();
        }

        private void AddCoins()
        {
            _sessionProgressService.Emeralds += _countCoins;
        }
    }
}