using System;
using System.Collections.Generic;

namespace Super_Auto_Mobs.Scripts
{
    [Serializable]
    public struct WorldData
    {
        public Title Title;
        public List<MobData> Command;
        public List<LevelData> LevelsData;
    }
}