using UnityEngine;

namespace Super_Auto_Mobs
{
    public class ShopCommandMobPlatform : ShopPlatform
    {
        public Mob Mob 
        { 
            get => (Mob)_entity;
            set
            {
                _entity = value;
            }
        }
    }
}