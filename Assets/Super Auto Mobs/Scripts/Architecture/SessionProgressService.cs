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
        public event Action<float> OnUpdateMusic;
        public event Action<float> OnUpdateSound;
        
        public int Gold
        {
            get => CurrentWorld.Emeralds;
            set
            {
                WorldProgress worldProgress = CurrentWorld;
                worldProgress.Emeralds = value;
                CurrentWorld = worldProgress;
                OnUpdateEmeralds?.Invoke(value);
            }
        }

        public int Hearts
        {
            get => CurrentWorld.Hearts;
            set
            {
                if (value > 0)
                {
                    WorldProgress worldProgress = CurrentWorld;
                    worldProgress.Hearts = value;
                    CurrentWorld = worldProgress;
                }
                
                OnUpdateHearts?.Invoke(value);
            }
        }
        
        public int Wins
        {
            get => CurrentWorld.Wins;
            set
            {
                WorldProgress worldProgress = CurrentWorld;
                
                if (value < CurrentWorldData.LevelsData.Count)
                {
                    worldProgress.Wins = value;
                }
                
                CurrentWorld = worldProgress;
                OnUpdateWins?.Invoke(value);
            }
        }

        public WorldData CurrentWorldData
        {
            get => _worldData;
        }

        private WorldData _worldData;
        
        public List<MobEnum> MobsUnlocked
        {
            get => CurrentWorld.MobsUnlocked;
            set
            {
                WorldProgress worldProgress = CurrentWorld;
                worldProgress.MobsUnlocked = value;
                CurrentWorld = worldProgress;
            }
        }

        public void AddMobUnlocked(MobEnum mobEnum)
        {
            bool isBe = false;
            
            foreach (var currentMobEnum in CurrentWorld.MobsUnlocked)
            {
                if (mobEnum == currentMobEnum)
                {
                    isBe = true;
                }
            }

            if (!isBe)
            {
                WorldProgress worldProgress = CurrentWorld;
                worldProgress.MobsUnlocked.Add(mobEnum);
                CurrentWorld = worldProgress;
            }
        }

        public List<BuffEnum> BuffsUnlocked
        {
            get => CurrentWorld.BuffsUnlocked;
            set
            {
                WorldProgress worldProgress = CurrentWorld;
                worldProgress.BuffsUnlocked = value;
                CurrentWorld = worldProgress;
            }
        }
        
        public void AddBuffUnlocked(BuffEnum buffEnum)
        {
            bool isBe = false;
            
            foreach (var currentBuffEnum in CurrentWorld.BuffsUnlocked)
            {
                if (buffEnum == currentBuffEnum)
                {
                    isBe = true;
                }
            }

            if (!isBe)
            {
                WorldProgress worldProgress = CurrentWorld;
                worldProgress.BuffsUnlocked.Add(buffEnum);
                CurrentWorld = worldProgress;
            }
        }
        
        public int ShopMobPlatformCountUnlock
        {
            get => CurrentWorld.ShopMobPlatformCountUnlock;
            set
            {
                WorldProgress worldProgress = CurrentWorld;
                worldProgress.ShopMobPlatformCountUnlock = value;
                CurrentWorld = worldProgress;
            }
        }
        
        public int ShopBuffPlatformCountUnlock
        {
            get => CurrentWorld.ShopBuffPlatformCountUnlock;
            set
            {
                WorldProgress worldProgress = CurrentWorld;
                worldProgress.ShopBuffPlatformCountUnlock = value;
                CurrentWorld = worldProgress;
            }
        }
        
        public List<MobData> MyCommandMobsData
        {
            get => CurrentWorld.MyCommandMobsData;
            set
            {
                WorldProgress worldProgress = CurrentWorld;
                worldProgress.MyCommandMobsData = value;
                CurrentWorld = worldProgress;
            }
        }

        public List<MobData> EnemyCommandMobsData => CurrentLevel.EnemyCommand;

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

        public GameData GameData => _gameData;
        public LevelData CurrentLevel => CurrentWorldData.LevelsData[IndexCurrentLevel];
        public int IndexCurrentWorld;
        public WorldProgress CurrentWorld
        {
            get => _gameData.WorldsProgress[IndexCurrentWorld];
            set => _gameData.WorldsProgress[IndexCurrentWorld] = value;
        }

        public int IndexCurrentLevel
        {
            get => CurrentWorld.IndexCurrentLevel;
            set
            {
                WorldProgress worldProgress = CurrentWorld;
                worldProgress.IndexCurrentLevel = value;
                CurrentWorld = worldProgress;
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
                IsFirsOpenGame = false;
            }
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
            OnUpdateEmeralds?.Invoke(CurrentWorld.Emeralds);
            OnUpdateHearts?.Invoke(CurrentWorld.Hearts);
            OnUpdateWins?.Invoke(CurrentWorld.Wins);
        }

        public void SetWorldData(WorldData worldData)
        {
            _worldData = worldData;

            for (int i = 0; i < _gameData.WorldsProgress.Count; i++)
            {
                if (_gameData.WorldsProgress[i].WorldEnum == worldData.WorldEnum)
                {
                    IndexCurrentWorld = i;
                    return;
                }
            }

            var progress = new WorldProgress
            {
                Hearts = worldData.MaxHealth,
                Wins = 0, 
                Emeralds = Constants.StartGold, 
                IndexCurrentLevel = 0,
                MyCommandMobsData = worldData.CommandData,
                MobsUnlocked = worldData.MobsUnlocked,
                BuffsUnlocked = worldData.BuffsUnlocked,
                ShopMobPlatformCountUnlock = worldData.ShopMobPlatformCountUnlock,
                ShopBuffPlatformCountUnlock = worldData.ShopBuffPlatformCountUnlock,
                WorldEnum = worldData.WorldEnum
            };

            _gameData.WorldsProgress.Add(progress);
        }
    }
}