using System;
using System.Collections.Generic;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class AssetProviderService : MonoBehaviour
    {
        public Material DefaultMaterial, OutlitMaterial, OutlitRedMaterial, DamageMaterial;
        public ShopCommandMobPlatform ShopCommandMobPlatform;
        public Spark Spark;
        
        [Header("Mobs")]
        public MobInfo DuckMob;
        public MobInfo OcelotMob;
        public MobInfo DogMob;
        public MobInfo VillageMob;
        public MobInfo SnowGolemMob;
        public MobInfo ZombieMob;
        public MobInfo SkeletonMob;
        public MobInfo ChoocobMob;
        public MobInfo BeeMob;
        public MobInfo EndermanMob;
        public MobInfo CowMob;
        public MobInfo VillagerBigMob;
        public MobInfo CowMilkMob;
        public MobInfo CreeperMob;
        public MobInfo WhitcherMob;
        public MobInfo SquidMob;
        public MobInfo TestMob;
        public MobInfo ChickenMob;
        public MobInfo PalesosMob;

        [Header("Bosses")]
        public Mob SquidBoss;

        public List<MobInfo> AllMobs;
        public List<Buff> Buffs;

        public MobInfo GetMobInfo(MobEnum mobEnum)
        {
            switch (mobEnum)
            {
                case MobEnum.Duck:
                    return DuckMob;
                case MobEnum.Cat:
                    return OcelotMob;
                case MobEnum.Dog:
                    return DogMob;
                case MobEnum.Villager:
                    return VillageMob;
                case MobEnum.SnowGollum:
                    return SnowGolemMob;
                case MobEnum.Zombie:
                    return ZombieMob;
                case MobEnum.Skeleton:
                    return SkeletonMob;
                case MobEnum.Chocobo:
                    return ChoocobMob;
                case MobEnum.Bee:
                    return BeeMob;
                case MobEnum.Enderman:
                    return EndermanMob;
                case MobEnum.MushroomCow:
                    return CowMob;
                case MobEnum.VillagerBig:
                    return VillagerBigMob;
                case MobEnum.MilkaCow:
                    return CowMilkMob;
                case MobEnum.Creeper:
                    return CreeperMob;
                case MobEnum.Witch:
                    return WhitcherMob;
                case MobEnum.Squid:
                    return SquidMob;
                case MobEnum.Test:
                    return TestMob;
                case MobEnum.Chicken:
                    return ChickenMob;
                case MobEnum.Palesos:
                    return PalesosMob;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}