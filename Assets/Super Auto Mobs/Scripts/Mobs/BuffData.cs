﻿using System;

namespace Super_Auto_Mobs
{
    [Serializable]
    public struct BuffData
    {
        public BuffEnum BuffEnum;
        public Title Name;
        public Title Info;
        public int Attack;
        public int Hearts;
        public EffectEnum EffectEnum;
        public BuffSoundEnum BuffSoundEnum;
        public bool IsSingle;
    }
}