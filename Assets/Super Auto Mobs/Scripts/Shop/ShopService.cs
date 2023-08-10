using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public abstract class ShopService : MonoBehaviour
    {
        public abstract event Action OnSelectCommandPlatform;
        public abstract event Action OnUnselectCommandPlatform;
        public abstract event Action<PlatformServiceState> OnUpdateState;
        public abstract List<ShopCommandMobPlatform> CommandPlatforms { get; }
        public abstract ShopPlatform ShopPlatformSelected { get; }
        public bool IsInteractive = true;
        public abstract void EnableBattleButton();

        public abstract void Open();
        public abstract void Close();
        public abstract Mob SpawnMob(MobData mobData);
        public abstract Buff SpawnBuff(BuffData buffData);
        public abstract void DestroySelectEntity();
        public abstract void DestroyPlatformMobs();
    }
}