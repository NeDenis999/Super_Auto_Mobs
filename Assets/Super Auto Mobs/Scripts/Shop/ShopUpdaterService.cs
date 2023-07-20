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

        public void UpdateShop()
        {
            RemoveAllMobs();
            CreateMobs();

            RemoveBuffs();
            CreateBuffs();
        }

        private void CreateMobs()
        {
            foreach (var platform in _shopMobPlatforms)
            {
                var randomMobInfo = _assetProviderService.AllMobs[Random.Range(0,
                    _assetProviderService.AllMobs.Count)];
                _mobFactoryService.CreateMobInPlatform(randomMobInfo.Prefab, platform, randomMobInfo.mobDefaultData);
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
        
        private void CreateBuffs()
        {
            foreach (var platform in _shopBuffPlatforms)
            {
                _mobFactoryService.CreateBuffInPlatform(_assetProviderService.Buffs[Random.Range(0,
                    _assetProviderService.Buffs.Count)], platform);
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