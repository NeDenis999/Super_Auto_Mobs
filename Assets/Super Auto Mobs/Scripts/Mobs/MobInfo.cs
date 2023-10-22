using UnityEngine;

namespace Super_Auto_Mobs
{
    [CreateAssetMenu(fileName = "Mob", menuName = "Data/Mob", order = 300)]
    public class MobInfo : ScriptableObject
    {
        public MobDefaultData mobDefaultData;
        public Mob Prefab;
    }
}