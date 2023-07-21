using System;
using System.Collections.Generic;

namespace Super_Auto_Mobs
{
    [Serializable]
    public struct ReplicaData
    {
        public SideEnum Side;
        public CharacterEnum Character;
        public List<Title> Replaces;
    }
}