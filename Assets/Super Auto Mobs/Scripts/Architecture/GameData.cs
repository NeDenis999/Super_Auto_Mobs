using System;
using System.Collections.Generic;

namespace Super_Auto_Mobs.Scripts
{
    [Serializable]
    public struct GameData
    {
        public List<MobData> MyCommandMobsData;
        public List<MobEnum> MobsUnlocked;
        public List<BuffEnum> BuffsUnlocked;
        public ProgressEnum ProgressEnum;
        public int Hearts;
        public int Wins;
        public int Emeralds;
        public int IndexCurrentWorld;
        public int IndexCurrentLevel;
        public int ShopMobPlatformCountUnlock;
        public int ShopBuffPlatformCountUnlock;
        public bool IsFirsOpenGame;
    }
}