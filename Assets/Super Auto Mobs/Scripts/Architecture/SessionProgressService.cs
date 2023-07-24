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
        public event Action<int> OnUpdateWins;
        
        public int Gold
        {
            get => _gameData.Emeralds;
            set
            {   
                _gameData.Emeralds = value;
                OnUpdateEmeralds?.Invoke(_gameData.Emeralds);
            }
        }
        
        public int Hearts
        {
            get => _gameData.Hearts;
            set
            {
                if (value <= 0)
                {
                    IndexCurrentLevel = 0;
                    Wins = 0;
                }
                else
                {
                    _gameData.Hearts = value;
                }
                
                OnUpdateHearts?.Invoke(_gameData.Hearts);
            }
        }
        
        public int Wins
        {
            get => _gameData.Wins;
            set
            {
                if (value >= CurrentWorld.MaxWins)
                {
                    _gameData.Wins = CurrentWorld.MaxWins;
                    IndexCurrentLevel++;
                }
                else
                {
                    IndexCurrentLevel = value;
                    _gameData.Wins = value;
                }

                OnUpdateWins?.Invoke(value);
            }
        }
        
        public int IndexCurrentLevel
        {
            get => _gameData.IndexCurrentLevel;
            set
            {
                if (value >= CurrentWorld.LevelsData.Count)
                {
                    IndexCurrentWorld++;
                    _gameData.IndexCurrentLevel = 0;
                }
                else
                {
                    _gameData.IndexCurrentLevel = value;
                }
            }
        }
        
        public int IndexCurrentWorld
        {
            get => _gameData.IndexCurrentWorld;
            set
            {
                _gameData.IndexCurrentLevel = 0;
                _gameData.Wins = 0;
                _gameData.Hearts = CurrentWorld.MaxHealth;
                
                if (value >= _gameData.Worlds.Count)
                {
                    _gameData.IndexCurrentWorld = _gameData.Worlds.Count - 1;
                }
                else
                {
                    _gameData.IndexCurrentWorld = value;
                }
            }
        }

        public WorldData CurrentWorld => _gameData.Worlds[IndexCurrentWorld].WorldData;
        public GameData GameData => _gameData;
        public LevelData CurrentLevel => CurrentWorld.LevelsData[IndexCurrentLevel];
        
        public List<MobEnum> MobsUnlocked
        {
            get => _gameData.MobsUnlocked;
            set
            {
                _gameData.MobsUnlocked = value;
            }
        }

        public int ShopMobPlatformCountUnlock
        {
            get => _gameData.ShopMobPlatformCountUnlock;
            set
            {
                _gameData.ShopMobPlatformCountUnlock = value;
            }
        }
        
        public int ShopBuffPlatformCountUnlock
        {
            get => _gameData.ShopBuffPlatformCountUnlock;
            set
            {
                _gameData.ShopBuffPlatformCountUnlock = value;
            }
        }

        public bool IsTest;
        public bool IsFirsOpenGame = true;
        public bool IsAutoPlay;
        public ProgressEnum ProgressEnum;
        public List<MobData> MyCommandMobsData;
        public List<MobData> EnemyCommandMobsData => CurrentLevel.EnemyCommand;

        [SerializeField]
        private GameData _gameData;

        private void Awake()
        {
            if (IsTest)
            {
                //Убрать загрузку
            }

            if (IsFirsOpenGame)
            {
                //Перезаписать все сохранения
                _gameData.ShopMobPlatformCountUnlock = 2;
                IsFirsOpenGame = false;
            }

            _gameData.Hearts = CurrentWorld.MaxHealth;
        }

        private void Start()
        {
            OnUpdateEmeralds?.Invoke(_gameData.Emeralds);
            OnUpdateHearts?.Invoke(_gameData.Hearts);
            OnUpdateWins?.Invoke(_gameData.Wins);
        }
    }
}