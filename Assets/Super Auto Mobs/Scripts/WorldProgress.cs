using System;
using System.Collections.Generic;

namespace Super_Auto_Mobs
{
    [Serializable]
    public struct WorldProgress
    {
        public WorldEnum WorldEnum;
        public int IndexCurrentLevel;
        public int Turn;
        public List<MobData> MyCommandMobsData;
        public List<MobEnum> MobsUnlocked;
        public List<BuffEnum> BuffsUnlocked;
        public int Hearts;
        public int Wins;
        public int Emeralds;
        public int ShopMobPlatformCountUnlock;
        public int ShopBuffPlatformCountUnlock;
        public bool IsEndWorld;
        public bool IsDisableBattleButton;
        public bool IsDisableRollButton;
        public bool IsCutSceneComplete;
        public bool IsDisableSellButton;
    }
}