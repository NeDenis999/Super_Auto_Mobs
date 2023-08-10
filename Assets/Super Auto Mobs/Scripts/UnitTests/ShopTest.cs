using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class ShopTest : ShopService
    {
        public override event Action OnSelectCommandPlatform;
        public override event Action OnUnselectCommandPlatform;
        public override event Action<PlatformServiceState> OnUpdateState;

        public override List<ShopCommandMobPlatform> CommandPlatforms => _commandPlatforms;
        public override ShopPlatform ShopPlatformSelected { get; }
        
        private AssetProviderService _assetProviderService;
        private List<ShopCommandMobPlatform> _commandPlatforms = new();
        private DiContainer _diContainer;

        [Inject]
        private void Construct(AssetProviderService assetProviderService,  DiContainer diContainer)
        {
            _assetProviderService = assetProviderService;
            _diContainer = diContainer;
        }

        public override void EnableBattleButton()
        {
            throw new NotImplementedException();
        }

        public override void Open()
        {
            gameObject.SetActive(true);
        }

        public override void Close()
        {
            gameObject.SetActive(false);
        }

        public override Mob SpawnMob(MobData mobData)
        {
            var platform = Instantiate(_assetProviderService.ShopCommandMobPlatform, transform);
            var mobInfo = _assetProviderService.GetMobInfo(mobData.MobEnum);
            var mob = Instantiate(mobInfo.Prefab, platform.transform);
            _diContainer.Inject(mob);
            _diContainer.Inject(mob.GetComponent<Perk>());
            _diContainer.Inject(platform);
            mob.Init(mobInfo.mobDefaultData, mobData);
            platform.Mob = mob;
            _commandPlatforms.Add(platform);
            return mob;
        }

        public override Buff SpawnBuff(BuffData buffData)
        {
            throw new NotImplementedException();
        }

        public override void DestroySelectEntity()
        {
            
        }

        public override void DestroyPlatformMobs()
        {
            for (int i = 0; i < _commandPlatforms.Count; i++)
            {
                Destroy(_commandPlatforms[i].gameObject);
            }
        }
    }
}