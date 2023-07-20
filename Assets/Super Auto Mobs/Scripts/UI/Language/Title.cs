using System;
using UnityEngine;

namespace Super_Auto_Mobs
{
    [Serializable]
    public struct Title
    {
        [TextArea(minLines:1, maxLines:100)]
        public string English;
        
        [TextArea(minLines:1, maxLines:100)]
        public string Russian;
    }
}