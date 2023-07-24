using System;
using System.Collections.Generic;
using UnityEngine;

namespace Super_Auto_Mobs
{
    [Serializable]
    public struct LevelData
    {
        public GameObject Background;
        public List<MobData> EnemyCommand;
        public List<Prize> Prizes;
    }
}