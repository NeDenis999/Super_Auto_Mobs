using UnityEngine;

namespace Super_Auto_Mobs
{
    [CreateAssetMenu(fileName = "Character", menuName = "Data/Character", order = 307)]
    public class CharacterData : ScriptableObject
    {
        public CharacterEnum Character;
        public Title Name;
        public Sprite View;
    }
}