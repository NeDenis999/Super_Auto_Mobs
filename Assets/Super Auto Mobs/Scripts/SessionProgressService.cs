using System;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class SessionProgressService : MonoBehaviour
    {
        public event Action<int> OnUpdateEmeralds;
        
        public int Emeralds
        {
            get => _emeralds;
            set
            {
                _emeralds = value;
                OnUpdateEmeralds?.Invoke(_emeralds);
            }
        }

        private int _emeralds;


        private void Start()
        {
            OnUpdateEmeralds?.Invoke(_emeralds);
        }
    }
}