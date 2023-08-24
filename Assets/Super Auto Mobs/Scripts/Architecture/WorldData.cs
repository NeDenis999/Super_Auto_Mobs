using System;
using System.Collections.Generic;

namespace Super_Auto_Mobs
{
    [Serializable]
    public struct WorldData
    {
        public WorldEnum WorldEnum;
        public Title Title;
        public int MaxHealth;
        public int ShopMobPlatformCountUnlock;
        public int ShopBuffPlatformCountUnlock;
        public List<MobData> CommandData; 
        public List<MobEnum> MobsUnlocked;
        public List<BuffEnum> BuffsUnlocked;
        public List<LevelData> LevelsData;
        public bool IsDisableBattleButton;
        public bool IsDisableRollButton;
        public bool IsDisableSellButton;
        public Dialogue DeathDialog;
    }
}