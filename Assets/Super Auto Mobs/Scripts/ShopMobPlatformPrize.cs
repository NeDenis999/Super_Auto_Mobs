using Zenject;

namespace Super_Auto_Mobs
{
    public class ShopMobPlatformPrize : Prize
    {
        private SessionProgressService _sessionProgressService;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService)
        {
            _sessionProgressService = sessionProgressService;
        }
        
        public override void Activate()
        {
            _sessionProgressService.ShopMobPlatformCountUnlock += 1;
        }
    }
}