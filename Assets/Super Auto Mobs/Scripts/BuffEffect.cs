using UnityEngine;

namespace Super_Auto_Mobs
{
    public abstract class BuffEffect : MonoBehaviour
    {
        public EffectEnum EffectEnum;
        public abstract void Activate(Mob mob);
        public abstract void Deactivate(Mob mob);
    }
}