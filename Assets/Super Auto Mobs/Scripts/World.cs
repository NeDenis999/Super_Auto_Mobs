using Super_Auto_Mobs.Scripts;
using UnityEngine;

namespace Super_Auto_Mobs
{
    [CreateAssetMenu(fileName = "World", menuName = "Data/World", order = 305)]
    public class World : ScriptableObject
    {
        public WorldData WorldData;
    }
}