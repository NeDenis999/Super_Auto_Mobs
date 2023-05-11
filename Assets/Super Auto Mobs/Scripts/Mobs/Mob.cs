using UnityEngine;

namespace Super_Auto_Mobs
{
    public class Mob : Entity
    {
        [SerializeField]
        private int _hearts;

        [SerializeField]
        private int _attack;

        private MobData _mobData;

        public void Init(MobData mobData)
        {
            _mobData = mobData;
        }
    }
}