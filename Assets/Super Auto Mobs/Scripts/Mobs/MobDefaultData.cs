using System;

namespace Super_Auto_Mobs
{
    [Serializable]
    public struct MobDefaultData
    {
        public Title Name;
        public Title Info;
        public int Hearts;
        public int Attack;
        public MobEnum MobEnum;
    }
}