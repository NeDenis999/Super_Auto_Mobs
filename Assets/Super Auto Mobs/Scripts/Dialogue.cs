using System.Collections.Generic;
using UnityEngine;

namespace Super_Auto_Mobs
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Data/Dialogue", order = 305)]
    public class Dialogue : ScriptableObject
    {
        public List<ReplicaData> ReplicasData;
    }
}