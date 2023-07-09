using System;
using System.Collections.Generic;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class AssetProviderService : MonoBehaviour
    {
        public Material DefaultMaterial, OutlitMaterial, OutlitRedMaterial, DamageMaterial;

        [Header("Mobs")]
        public Mob ChickenMob;
        public Mob OcelotMob;
        public Mob DogMob;
        public Mob VillageMob;
        public Mob SnowGolemMob;
        public Mob ZombieMob;
        public Mob SkeletonMob;
        public Mob ChoocobMob;
        public Mob BeeMob;
        public Mob EndermanMob;
        public Mob CowMob;
        public Mob VillagerBigMob;
        public Mob CowMilkMob;
        public Mob CreeperMob;
        public Mob WhitcherMob;
        public Mob SquidMob;
        public Mob TestMob;

        [Header("Bosses")]
        public Mob SquidBoss;

        public List<Mob> AllMobs;
        public List<Buff> Buffs;

        public Mob MobPrefab(MobEnum mobEnum)
        {
            switch (mobEnum)
            {
                case MobEnum.Chicken:
                    return ChickenMob;
                case MobEnum.Ocelot:
                    return OcelotMob;
                case MobEnum.Dog:
                    return DogMob;
                case MobEnum.Village:
                    return VillageMob;
                case MobEnum.SnowGolem:
                    return SnowGolemMob;
                case MobEnum.Zombie:
                    return ZombieMob;
                case MobEnum.Skeleton:
                    return SkeletonMob;
                case MobEnum.Choocob:
                    return ChoocobMob;
                case MobEnum.Bee:
                    return BeeMob;
                case MobEnum.Enderman:
                    return EndermanMob;
                case MobEnum.Cow:
                    return CowMob;
                case MobEnum.VillagerBig:
                    return VillagerBigMob;
                case MobEnum.CowMilk:
                    return CowMilkMob;
                case MobEnum.Creeper:
                    return CreeperMob;
                case MobEnum.Whitcher:
                    return WhitcherMob;
                case MobEnum.Squid:
                    return SquidMob;
                case MobEnum.Test:
                    return TestMob;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}