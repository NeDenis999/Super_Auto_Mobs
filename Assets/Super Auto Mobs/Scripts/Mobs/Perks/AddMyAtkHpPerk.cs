using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class AddMyAtkHpPerk : Perk
    {
        [SerializeField]
        private int _atack = 1;
        
        [SerializeField]
        private int _health = 1;

        public override void Activate()
        {
            print("AddMyAtkHpPerk Activate");
            var mob = GetComponent<Mob>();

            mob.ChangeAttack(_atack);
            mob.ChangeHearts(_health);
        }
    }
}