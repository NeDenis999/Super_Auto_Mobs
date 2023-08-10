using System;
using System.Collections.Generic;
using UnityEngine;

namespace Super_Auto_Mobs
{
    [Serializable]
    public struct LevelData
    {
        public Location ShopLocation;
        public Location BattleLocation;
        public List<MobData> EnemyCommand;
        public List<Prize> Prizes;
    }
}