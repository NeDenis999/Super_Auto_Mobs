using System;
using System.Collections.Generic;
using Super_Auto_Mobs.Scripts;
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

        public List<MobData> MyCommandMobsData => _myCommandMobsData;
        public List<MobData> EnemyCommandMobsData => _enemyCommandMobsData;
        public GameData GameData => _gameData;

        [SerializeField]
        private List<MobData> _myCommandMobsData;
        
        [SerializeField]
        private List<MobData> _enemyCommandMobsData;

        [SerializeField]
        private GameData _gameData;
        
        private int _emeralds;
        
        private void Start()
        {
            OnUpdateEmeralds?.Invoke(_emeralds);
        }
    }
}