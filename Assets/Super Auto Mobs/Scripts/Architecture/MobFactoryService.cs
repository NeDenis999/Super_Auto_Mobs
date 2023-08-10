using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class MobFactoryService : MonoBehaviour
    {
        private DiContainer _diContainer;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
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
    }
}