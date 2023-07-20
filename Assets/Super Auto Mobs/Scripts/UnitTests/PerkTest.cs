using System.Collections;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class PerkTest : Perk
    {
        public override IEnumerator Activate()
        {
            print("PerkTest");    
            
            yield break;
        }
    }
}