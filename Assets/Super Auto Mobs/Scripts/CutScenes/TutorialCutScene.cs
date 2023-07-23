using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class TutorialCutScene : CutScene
    {
        private SessionProgressService _sessionProgressService;
        private ShopUpdaterService _shopUpdaterService;
        private AssetProviderService _assetProviderService;
        private ShopService _shopService;
        
        [Inject]
        private void Construct(SessionProgressService sessionProgressService, ShopUpdaterService shopUpdaterService,
            AssetProviderService assetProviderService, ShopService shopService)
        {
            _sessionProgressService = sessionProgressService;
            _shopUpdaterService = shopUpdaterService;
            _assetProviderService = assetProviderService;
            _shopService = shopService;
        }
        
        public override void Play()
        {
            base.Play();
            _sessionProgressService.MyCommandMobsData = new List<MobData>();
            _sessionProgressService.EnemyCommandMobsData = new List<MobData>()
            {
                new MobData() {MobEnum = MobEnum.Palesos}
            };
            
            _shopService.DestroyPlatformMobs();
            _shopUpdaterService.UpdateShop(new List<MobInfo>() {_assetProviderService.ChickenMob}, new List<Buff>());
        }
    }
}