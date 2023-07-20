using UnityEngine;

namespace Super_Auto_Mobs
{
    [CreateAssetMenu(fileName = "Entity", menuName = "Mob/Entity", order = 301)]
    public class EntityInfo : ScriptableObject
    {
        public FoodData foodData;
    }
}