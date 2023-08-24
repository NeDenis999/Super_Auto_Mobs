using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class MobFactoryService : MonoBehaviour
    {
        private DiContainer _diContainer;
        private AssetProviderService _assetProviderService;

        [Inject]
        private void Construct(DiContainer diContainer, AssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
            _diContainer = diContainer;
        }

        public Mob CreateMobInPlatform(Mob mobPref, ShopPlatform platform, MobDefaultData mobDefaultData, MobData mobData)
        {
            var mob = Instantiate(mobPref, platform.SpawnPoint.position, Quaternion.identity, platform.SpawnPoint);
            platform.Entity = mob;
                
            _diContainer.Inject(mob);
            
            if (mob.GetComponent<Perk>() == null)
                mob.gameObject.AddComponent<AddCoinsPerk>();
            
            _diContainer.Inject(mob.GetComponent<Perk>());
            
            mob.Init(mobDefaultData, mobData);
            
            if (mobData.EffectEnum != EffectEnum.None)
            {
                CreateBuffEffect(mobData.EffectEnum, mob);
            }
            
            return mob;
        }

        public Buff CreateBuffInPlatform(BuffInfo buffInfo, ShopBuffPlatform platform)
        {
            var buff = Instantiate(buffInfo.Prefab, platform.SpawnPoint.position, Quaternion.identity, platform.SpawnPoint);
            platform.Entity = buff;
            buff.Init(buffInfo.BuffData);

            _diContainer.Inject(buff);
            
            return buff;
        }
        
        public Mob SpawnMob(MobData mobData, bool isEnemy, Transform spawnTransform)
        {
            var mobInfo = _assetProviderService.GetMobInfo(mobData.MobEnum);
            var mob = Instantiate(mobInfo.Prefab, spawnTransform);
            _diContainer.Inject(mob);
            var perk = mob.GetComponent<Perk>();
            
            if (perk)
                _diContainer.Inject(perk);
            
            mob.Init(mobInfo.mobDefaultData, mobData, isEnemy);
            
            if (mobData.EffectEnum != EffectEnum.None)
            {
                CreateBuffEffect(mobData.EffectEnum, mob);
            }
            
            return mob;
        }

        public void CreateBuffEffect(EffectEnum buffDataEffectEnum, Mob mob)
        {
            var effect = Instantiate(_assetProviderService.GetBuffEffect(buffDataEffectEnum), mob.transform);
            mob.SetBuffEffect(effect);
        }
    }
}