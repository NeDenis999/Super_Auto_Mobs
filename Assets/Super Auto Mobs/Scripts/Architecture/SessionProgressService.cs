using System;
using System.Collections.Generic;
using Super_Auto_Mobs.Scripts;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class SessionProgressService : MonoBehaviour
    {
        public event Action<int> OnUpdateEmeralds;
        public event Action<int> OnUpdateHearts;
        public event Action<int, int> OnUpdateWins;
        
        public int Emeralds
        {
            get => _emeralds;
            set
            {   
                _emeralds = value;
                OnUpdateEmeralds?.Invoke(_emeralds);
            }
        }
        
        public int Hearts
        {
            get => _hearts;
            set
            {   
                _hearts = value;
                OnUpdateHearts?.Invoke(_hearts);
            }
        }
        
        public int Wins
        {
            get => _wins;
            set
            {   
                _wins = value;
                OnUpdateWins?.Invoke(_wins, _maxWins);
            }
        }

        public bool IsTest;
        public bool IsAutoPlay;
        public bool IsTutorialComplete;
        public List<MobData> MyCommandMobsData;
        public List<MobData> EnemyCommandMobsData;
        public GameData GameData => _gameData;

        [SerializeField]
        private GameData _gameData;
        
        private int _emeralds;
        private int _hearts;
        private int _wins;
        private int _maxWins;
        
        private void Start()
        {
            if (IsTest)
            {
                //Убрать загрузку
            }
            
            OnUpdateEmeralds?.Invoke(_emeralds);
            OnUpdateHearts?.Invoke(_hearts);
            OnUpdateWins?.Invoke(_wins, _maxWins);
        }
    }
}