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

        public Buff CreateBuffInPlatform(Buff buffPref, ShopBuffPlatform platform)
        {
            var buff = Instantiate(buffPref, platform.SpawnPoint.position, Quaternion.identity, platform.SpawnPoint);
            platform.Entity = buff;
            
            _diContainer.Inject(buff);
            
            return buff;
        }
    }
}