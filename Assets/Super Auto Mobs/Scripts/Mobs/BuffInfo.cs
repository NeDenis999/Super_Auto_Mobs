using UnityEngine;

namespace Super_Auto_Mobs
{
    [CreateAssetMenu(fileName = "Buff", menuName = "Data/Buff", order = 301)]
    public class BuffInfo : ScriptableObject
    {
        public BuffData BuffData;
        public Buff Prefab;
    }
}