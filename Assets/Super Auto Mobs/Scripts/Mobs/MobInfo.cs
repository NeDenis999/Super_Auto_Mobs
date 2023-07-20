using UnityEngine;

namespace Super_Auto_Mobs
{
    [CreateAssetMenu(fileName = "Mob", menuName = "Mob/Mob", order = 300)]
    public class MobInfo : ScriptableObject
    {
        public MobDefaultData mobDefaultData;
        public Mob Prefab;
    }
}