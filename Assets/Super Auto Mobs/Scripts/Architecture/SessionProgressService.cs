using System;
using System.Collections.Generic;
using Super_Auto_Mobs.Scripts;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class SessionProgressService : MonoBehaviour
    {
        public event Action<int> OnUpdateEmeralds;
        public event Action<int> OnUpdateHearts;
        public event Action<int> OnUpdateWins;
        public event Action<bool> OnUpdateIsAutoPlay;
        
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
                    //IndexCurrentWorld++;
                    //_gameData.IndexCurrentLevel = 0;
                }
                else
                {
                    _gameData.IndexCurrentLevel = value;
                }
            }
        }
        
        /*public int IndexCurrentWorld
        {
            get => _gameData.IndexCurrentWorld;
            set
            {
                _gameData.IndexCurrentLevel = 0;
                _gameData.Wins = 0;
                _gameData.Hearts = CurrentWorld.MaxHealth;
                
                if (value >= _assetProviderService.Worlds.Count)
                {
                    _gameData.IndexCurrentWorld = _assetProviderService.Worlds.Count - 1;
                }
                else
                {
                    _gameData.IndexCurrentWorld = value;
                }
            }
        }*/

        public WorldData CurrentWorld
        {
            get { return _assetProviderService.Worlds[_game.IndexCurrentWorld].WorldData; }
            set
            {
                for (int i = 0; i < _assetProviderService.Worlds.Count; i++)
                {
                    if (_assetProviderService.Worlds[i].WorldData.Equals(value))
                    {
                        _game.IndexCurrentWorld = i;
                    }
                }
            }
        }

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
        
        public void AddMobUnlocked(MobEnum mobEnum)
        {
            bool isBe = false;
            
            foreach (var currentMobEnum in _gameData.MobsUnlocked)
            {
                if (mobEnum == currentMobEnum)
                {
                    isBe = true;
                }
            }
            
            if (!isBe)
                _gameData.MobsUnlocked.Add(mobEnum);
        }

        public List<BuffEnum> BuffsUnlocked
        {
            get => _gameData.BuffsUnlocked;
            set
            {
                _gameData.BuffsUnlocked = value;
            }
        }
        
        public void AddBuffUnlocked(BuffEnum buffEnum)
        {
            bool isBe = false;
            
            foreach (var currentBuffEnum in _gameData.BuffsUnlocked)
            {
                if (buffEnum == currentBuffEnum)
                {
                    isBe = true;
                }
            }
            
            if (!isBe)
                _gameData.BuffsUnlocked.Add(buffEnum);
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
        
        public List<MobData> MyCommandMobsData
        {
            get { return _gameData.MyCommandMobsData; }
            set
            {
                _gameData.MyCommandMobsData = value;
            }
        }

        public List<MobData> EnemyCommandMobsData => CurrentLevel.EnemyCommand;
        public event Action<float> OnUpdateMusic;
        public event Action<float> OnUpdateSound;

        public float Music
        {
            get => _settingsData.Music;

            set
            {
                OnUpdateMusic?.Invoke(_settingsData.Music);
                _settingsData.Music = value;
            }
        }
        
        public float Sound
        {
            get => _settingsData.Sound;

            set
            {
                OnUpdateSound?.Invoke(_settingsData.Sound);
                _settingsData.Sound = value;
            }
        }

        public bool IsAutoPlay
        {
            get => _settingsData.IsAutoPlay;

            set
            {
                OnUpdateIsAutoPlay?.Invoke(_settingsData.IsAutoPlay);
                _settingsData.IsAutoPlay = value;
            }
        }
        
        public LanguageService.Language Language
        {
            get => _settingsData.Language;

            set
            {
                //OnUpdateIsAutoPlay?.Invoke(_settingsData.IsAutoPlay);
                _settingsData.Language = value;
            }
        }
        
        public bool IsFirsOpenGame
        {
            get => _gameData.IsFirsOpenGame;

            set
            {
                //OnUpdateIsAutoPlay?.Invoke(_settingsData.IsAutoPlay);
                _gameData.IsFirsOpenGame = value;
            }
        }
            
        public ProgressEnum ProgressEnum
        {
            get => _gameData.ProgressEnum;

            set
            {
                //OnUpdateIsAutoPlay?.Invoke(_settingsData.IsAutoPlay);
                _gameData.ProgressEnum = value;
            }
        }

        public bool IsTest;

        [SerializeField]
        private AssetProviderService _assetProviderService;

        [SerializeField]
        private Game _game;
        
        [SerializeField]
        private GameData _gameData;

        [SerializeField]
        private SettingsData _settingsData;
        
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

        private void OnEnable()
        {
            SettingsData defaultSettingsData = new SettingsData()
            {
                Sound = 1f,
                Music = 1f,
                IsAutoPlay = false
            };

            var textDefaultSettingData = JsonConvert.SerializeObject(defaultSettingsData);
            
            _settingsData = JsonConvert.DeserializeObject<SettingsData>(PlayerPrefs.GetString("Setting", textDefaultSettingData));
            print(_settingsData);
        }

        private void OnDisable()
        {
            PlayerPrefs.SetString("Setting", JsonConvert.SerializeObject(_settingsData));
        }

        private void Start()
        {
            OnUpdateEmeralds?.Invoke(_gameData.Emeralds);
            OnUpdateHearts?.Invoke(_gameData.Hearts);
            OnUpdateWins?.Invoke(_gameData.Wins);
        }
    }
}