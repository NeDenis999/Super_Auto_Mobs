using System.Collections;
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
        
        public override IEnumerator Activate()
        {
            print("AddCoinsPerk Activate");
            AddCoins();
            
            yield break;
        }

        private void AddCoins()
        {
            _sessionProgressService.Gold += _countCoins;
        }
    }
}