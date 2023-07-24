using System;
using System.Collections.Generic;

namespace Super_Auto_Mobs.Scripts
{
    [Serializable]
    public struct GameData
    {
        public int Hearts;
        public int Wins;
        public int Emeralds;
        public int IndexCurrentWorld;
        public int IndexCurrentLevel;
        public List<World> Worlds;
        public List<MobEnum> MobsUnlocked;
        public int ShopMobPlatformCountUnlock;
        public int ShopBuffPlatformCountUnlock;
    }
}