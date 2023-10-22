using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Super_Auto_Mobs
{
    public class ShopUpdaterService : MonoBehaviour
    {
        [SerializeField]
        private List<ShopMobPlatform> _shopMobPlatforms;

        [SerializeField]
        private List<ShopBuffPlatform> _shopBuffPlatforms;
        
        private MobFactoryService _mobFactoryService;
        private AssetProviderService _assetProviderService;
        private List<ShopMobPlatform> _shopMobPlatformsUnlock;
        private List<ShopBuffPlatform> _shopBuffPlatformsUnlock;
        private SessionProgressService _sessionProgressService;

        [Inject]
        private void Construct(MobFactoryService mobFactoryService, AssetProviderService assetProviderService, 
            SessionProgressService sessionProgressService)
        {
            _mobFactoryService = mobFactoryService;
            _assetProviderService = assetProviderService;
            _sessionProgressService = sessionProgressService;
        }

        public void UpdateShop(List<MobInfo> mobs = null, List<BuffInfo> buffs = null)
        {
            _shopMobPlatformsUnlock = new List<ShopMobPlatform>();
            _shopBuffPlatformsUnlock = new List<ShopBuffPlatform>();

            foreach (var platform in _shopMobPlatforms)
            {
                platform.gameObject.SetActive(false);
            }
            
            for (int i = 0; i < _sessionProgressService.ShopMobPlatformCountUnlock; i++)
            {
                _shopMobPlatformsUnlock.Add(_shopMobPlatforms[i]);
                _shopMobPlatforms[i].gameObject.SetActive(true);
            }
            
            foreach (var platform in _shopBuffPlatforms)
            {
                platform.gameObject.SetActive(false);
            }
            
            for (int i = 0; i < _sessionProgressService.ShopBuffPlatformCountUnlock; i++)
            {
                _shopBuffPlatformsUnlock.Add(_shopBuffPlatforms[i]);
                _shopBuffPlatforms[i].gameObject.SetActive(true);
            }
            
            if (mobs == null)
            {
                mobs = new List<MobInfo>();
                
                foreach (var platform in _shopMobPlatformsUnlock)
                {
                    if (_sessionProgressService.MobsUnlocked.Count == 0)
                        break;
                    
                    var randomMobInfo = _assetProviderService.GetMobInfo(_sessionProgressService.MobsUnlocked[Random.Range(0,
                        _sessionProgressService.MobsUnlocked.Count)]);
                    
                    mobs.Add(randomMobInfo);
                }
            }

            if (buffs == null)
            {
                buffs = new List<BuffInfo>();

                foreach (var platform in _shopBuffPlatformsUnlock)
                {
                    if (_sessionProgressService.BuffsUnlocked.Count == 0)
                        break;
                    
                    var randomBuffInfo = _assetProviderService.GetBuffInfo(_sessionProgressService.BuffsUnlocked[Random.Range(0,
                        _sessionProgressService.BuffsUnlocked.Count)]);
                    
                    buffs.Add(randomBuffInfo);
                    
                    //buffs.Add(_assetProviderService.Buffs[Random.Range(0,
                        //_assetProviderService.Buffs.Count)]);
                }
            }
            
            RemoveAllMobs();
            SpawnMobs(mobs);

            RemoveBuffs();
            SpawnBuffs(buffs);
        }

        public void SpawnMob(MobEnum mobEnum)
        {
            for (int i = 0; i < _shopMobPlatformsUnlock.Count; i++)
            {
                if (!_shopMobPlatformsUnlock[i].IsEntity)
                {
                    var mobInfo = _assetProviderService.GetMobInfo(mobEnum);
                    var mobData = new MobData()
                    {
                        MobEnum = mobInfo.mobDefaultData.MobEnum
                    };
                    
                    _mobFactoryService.CreateMobInPlatform(mobInfo.Prefab, _shopMobPlatformsUnlock[i], mobInfo.mobDefaultData, mobData);
                    return;
                }
            }
        }
        
        private void SpawnMobs(List<MobInfo> mobs)
        {
            for (int i = 0; i < _shopMobPlatformsUnlock.Count; i++)
            {
                if (i < mobs.Count)
                {
                    var mobData = new MobData
                    {
                        MobEnum = mobs[i].mobDefaultData.MobEnum
                    };
                    
                    _mobFactoryService.CreateMobInPlatform(mobs[i].Prefab, _shopMobPlatformsUnlock[i], mobs[i].mobDefaultData, mobData);
                }
            }
        }

        private void RemoveAllMobs()
        {
            foreach (var platform in _shopMobPlatformsUnlock)
            {
                if (platform.IsEntity)
                {
                    Destroy(platform.Entity.gameObject);
                    platform.Entity = null;
                }
            }
        }
        
        public void SpawnBuff(BuffEnum buffEnum)
        {
            for (int i = 0; i < _shopBuffPlatformsUnlock.Count; i++)
            {
                if (!_shopBuffPlatformsUnlock[i].IsEntity)
                {
                    var buffInfo = _assetProviderService.GetBuffInfo(buffEnum);
                    _mobFactoryService.CreateBuffInPlatform(buffInfo, _shopBuffPlatformsUnlock[i]);
                    return;
                }
            }
        }
        
        private void SpawnBuffs(List<BuffInfo> buffs)
        {
            for (int i = 0; i < _shopBuffPlatformsUnlock.Count; i++)
            {
                if (i < buffs.Count)
                    _mobFactoryService.CreateBuffInPlatform(buffs[i], _shopBuffPlatformsUnlock[i]);
            }
        }
        
        private void RemoveBuffs()
        {
            foreach (var platform in _shopBuffPlatformsUnlock)
            {
                if (platform.IsEntity)
                {
                    Destroy(platform.Entity.gameObject);
                    platform.Entity = null;
                }
            }
        }
    }
}