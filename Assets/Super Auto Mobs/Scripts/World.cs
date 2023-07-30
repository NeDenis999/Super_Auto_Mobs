using UnityEngine;

namespace Super_Auto_Mobs
{
    [CreateAssetMenu(fileName = "World", menuName = "Data/World", order = 305)]
    public class World : ScriptableObject
    {
        public Sprite Preview;
        public WorldData WorldData;
    }
}