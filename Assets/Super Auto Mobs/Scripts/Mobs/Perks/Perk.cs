using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public abstract class Perk : MonoBehaviour
    {
        [SerializeField]
        private TriggeringSituation _triggeringSituation;

        public TriggeringSituation TriggeringSituation => _triggeringSituation;
        public abstract IEnumerator Activate();
    }
}