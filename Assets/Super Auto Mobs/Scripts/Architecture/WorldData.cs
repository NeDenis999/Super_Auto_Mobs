using System;
using System.Collections.Generic;

namespace Super_Auto_Mobs
{
    [Serializable]
    public struct WorldData
    {
        public Title Title;
        public int MaxHealth => LevelsData.Count;
        public int MaxWins => LevelsData.Count;
        public List<LevelData> LevelsData;
    }
}