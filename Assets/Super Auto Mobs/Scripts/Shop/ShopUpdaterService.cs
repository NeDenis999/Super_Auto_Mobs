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
        
        public List<ShopMobPlatform> ShopMobPlatforms => _shopMobPlatforms;

        [Inject]
        private void Construct(MobFactoryService mobFactoryService, AssetProviderService assetProviderService)
        {
            _mobFactoryService = mobFactoryService;
            _assetProviderService = assetProviderService;
        }

        public void UpdateShop(List<MobInfo> mobs = null, List<Buff> buffs = null)
        {
            if (mobs == null)
            {
                mobs = new List<MobInfo>();
                
                foreach (var platform in _shopMobPlatforms)
                {
                    var randomMobInfo = _assetProviderService.AllMobs[Random.Range(0,
                        _assetProviderService.AllMobs.Count)];
                    
                    mobs.Add(randomMobInfo);
                }
            }

            if (buffs == null)
            {
                buffs = new List<Buff>();

                foreach (var platform in _shopBuffPlatforms)
                {
                    buffs.Add(_assetProviderService.Buffs[Random.Range(0,
                        _assetProviderService.Buffs.Count)]);
                }
            }
            
            RemoveAllMobs();
            CreateMobs(mobs);

            RemoveBuffs();
            CreateBuffs(buffs);
        }

        private void CreateMobs(List<MobInfo> mobs)
        {
            for (int i = 0; i < _shopMobPlatforms.Count; i++)
            {
                if (i < mobs.Count)
                {
                    var mobData = new MobData
                    {
                        MobEnum = mobs[i].mobDefaultData.MobEnum
                    };
                    
                    _mobFactoryService.CreateMobInPlatform(mobs[i].Prefab, _shopMobPlatforms[i], mobs[i].mobDefaultData, mobData);
                }
            }
        }
        
        private void RemoveAllMobs()
        {
            foreach (var platform in _shopMobPlatforms)
            {
                if (platform.IsEntity)
                {
                    Destroy(platform.Entity.gameObject);
                    platform.Entity = null;
                }
            }
        }
        
        private void CreateBuffs(List<Buff> buffs)
        {
            for (int i = 0; i < _shopBuffPlatforms.Count; i++)
            {
                if (i < buffs.Count)
                    _mobFactoryService.CreateBuffInPlatform(buffs[i], _shopBuffPlatforms[i]);
            }
        }
        
        private void RemoveBuffs()
        {
            foreach (var platform in _shopBuffPlatforms)
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