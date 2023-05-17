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
        private int _currentHearts;

        public bool IsActive => _currentHearts > 0;

        public void Init(MobData mobData)
        {
            _mobData = mobData;
            _currentHearts = _hearts + mobData.Hearts;
        }
    }
}