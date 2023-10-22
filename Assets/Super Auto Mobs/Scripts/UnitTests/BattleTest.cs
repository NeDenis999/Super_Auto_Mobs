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

        public override Mob SpawnMob(MobData mobData, bool isEnemy, bool mobIsEnemy = false)
        {
            var list = isEnemy ? _enemyCommandMobs : _myCommandMobs;

            var mobInfo = _assetProviderService.GetMobInfo(mobData.MobEnum);
            var mob = Instantiate(mobInfo.Prefab, transform);
            _diContainer.Inject(mob);
            
            var perk = mob.GetComponent<Perk>();

            if (perk)
                _diContainer.Inject(perk);
                    
            mob.Init(mobInfo.mobDefaultData, mobData, isEnemy);
            list.Add(mob);
            return mob;
        }

        public override IEnumerator AwaitIntro()
        {
            yield break;
        }

        public override IEnumerator AwaitProcessBattle()
        {
            foreach (var myCommandMob in _myCommandMobs)
            {
                if (myCommandMob.Perk.TriggeringSituation == TriggeringSituation.StartBattle)
                {
                    StartCoroutine(myCommandMob.Perk.Activate());
                }
            }
            
            foreach (var enemyCommandMob in _enemyCommandMobs)
            {
                if (enemyCommandMob.Perk.TriggeringSituation == TriggeringSituation.StartBattle)
                {
                    StartCoroutine(enemyCommandMob.Perk.Activate());
                }
            }
            
            yield break;
        }

        public override IEnumerator Attack(bool isEnemy)
        {
            var activeMob = isEnemy ? GetMobActive(_enemyCommandMobs) : GetMobActive(_myCommandMobs);
            var oppositeActiveMob = isEnemy ? GetMobActive(_myCommandMobs) : GetMobActive(_enemyCommandMobs);

            if (activeMob.Perk.TriggeringSituation == TriggeringSituation.Attack)
            {
                yield return activeMob.Perk.Activate();
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
        
        public override void RemovePets()
        {
            foreach (var mob in _myCommandMobs)
            {
                Destroy(mob.gameObject);
            }

            foreach (var mob in _enemyCommandMobs)
            {
                Destroy(mob.gameObject);
            }

            _myCommandMobs.RemoveAll(IsMobRemove);
            _enemyCommandMobs.RemoveAll(IsMobRemove);
        }
        
        private static bool IsMobRemove(Mob mob)
        {
            return true;
        }
    }
}