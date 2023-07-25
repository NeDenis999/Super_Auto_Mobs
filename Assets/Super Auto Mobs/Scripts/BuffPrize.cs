using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class BuffPrize : Prize
    {
        [SerializeField]
        private BuffEnum _buffEnum;

        private SessionProgressService _sessionProgressService;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService)
        {
            _sessionProgressService = sessionProgressService;
        }
        
        public override void Activate()
        {
            _sessionProgressService.AddBuffUnlocked(_buffEnum);
        }
    }
}