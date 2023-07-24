using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class MobPrize : Prize
    {
        [SerializeField]
        private MobEnum _mobEnum;

        private SessionProgressService _sessionProgressService;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService)
        {
            _sessionProgressService = sessionProgressService;
        }
        
        public override void Activate()
        {
            _sessionProgressService.MobsUnlocked.Add(_mobEnum);
        }
    }
}