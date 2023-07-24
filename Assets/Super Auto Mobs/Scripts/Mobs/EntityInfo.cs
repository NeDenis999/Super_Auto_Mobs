using UnityEngine;

namespace Super_Auto_Mobs
{
    [CreateAssetMenu(fileName = "Entity", menuName = "Data/Entity", order = 301)]
    public class EntityInfo : ScriptableObject
    {
        public FoodData foodData;
    }
}