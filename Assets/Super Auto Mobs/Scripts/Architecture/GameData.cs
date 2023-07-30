using System;
using System.Collections.Generic;

namespace Super_Auto_Mobs.Scripts
{
    [Serializable]
    public struct GameData
    {
        public List<WorldProgress> WorldsProgress;
        public int IndexLastWorld;
        public bool IsFirsOpenGame;
    }
}