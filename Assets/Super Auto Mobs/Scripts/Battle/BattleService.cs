using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public abstract class BattleService : MonoBehaviour
    {
        public abstract List<Mob> MyCommandMobs { get; }

        public abstract List<Mob> EnemyCommandMobs { get; }
        
        public abstract void Open();

        public abstract void Close();
        public abstract void RemovePets();
        public abstract IEnumerator AwaitIntro();
        public abstract IEnumerator AwaitProcessBattle();
        public abstract IEnumerator Attack(bool isEnemy);

        public abstract Mob SpawnMob(MobData mobData, bool isEnemy, bool mobIsEnemy = false);
    }
}