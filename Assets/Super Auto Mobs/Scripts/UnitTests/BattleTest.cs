using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace Super_Auto_Mobs
{
    public class BattleTest : BattleService
    {
        private List<Mob> _myCommandMobs = new();
        private List<Mob> _enemyCommandMobs = new();
        private AssetProviderService _assetProviderService;
        private DiContainer _diContainer;

        public override List<Mob> MyCommandMobs => _myCommandMobs;
        public override List<Mob> EnemyCommandMobs => _enemyCommandMobs;

        [Inject]
        private void Construct(AssetProviderService assetProviderService, DiContainer diContainer)
        {
            _assetProviderService = assetProviderService;
            _diContainer = diContainer;
        }
        
        public override void Open()
        {
            gameObject.SetActive(true);
        }

        public override void Close()
        {
            gameObject.SetActive(false);
        }

        public override void SpawnMob(MobData mobData, bool isEnemy)
        {
            var list = isEnemy ? _enemyCommandMobs : _myCommandMobs;

            var mob = Instantiate(_assetProviderService.MobPrefab(mobData.MobEnum), transform);
            _diContainer.Inject(mob);
            _diContainer.Inject(mob.GetComponent<Perk>());
            mob.Init(mobData, isEnemy);
            list.Add(mob);
        }

        public override IEnumerator AwaitIntro()
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerator AwaitProcessBattle()
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerator Attack(bool isEnemy)
        {
            var activeMob = isEnemy ? GetMobActive(_enemyCommandMobs) : GetMobActive(_myCommandMobs);
            var oppositeActiveMob = isEnemy ? GetMobActive(_myCommandMobs) : GetMobActive(_enemyCommandMobs);

            if (activeMob.Perk.TriggeringSituation == TriggeringSituation.Attack)
            {
                activeMob.Perk.Activate();
            }
            
            oppositeActiveMob.TakeDamage(activeMob.CurrentAttack);
            
            yield return null;
        }
        
        private Mob GetMobActive(List<Mob> mobs)
        {
            foreach (var mob in mobs)
                if (mob.IsActive)
                    return mob;

            return null;
        }
    }
}