using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class AssetProviderService : MonoBehaviour
    {
        public Material DefaultMaterial, OutlitMaterial, OutlitRedMaterial, DamageMaterial;
        public ShopCommandMobPlatform ShopCommandMobPlatform;
        public Spark Spark;
        
        [Header("Data")]
        public List<World> Worlds;
        public List<MobInfo> Mobs;
        public List<BuffInfo> Buffs;
        public List<CharacterData> CharactersData;
        public List<BuffEffect> BuffEffects;

        [Header("Sprites")]
        public Sprite AttackSprite;
        public Sprite HeartSprite;
        public Sprite DamageSprite;
        public Sprite ActivatePerkSprite;

        [Header("Other")]
        public MobInfo SquidBoss;

        [MenuItem("Tools/UpdateAllData")]
        public static void UpdateAllData()
        {
            var assetProviderService = FindObjectOfType<AssetProviderService>();
            
            assetProviderService.Mobs = new List<MobInfo>();
            
            foreach (MobInfo mobInfo in Resources.LoadAll<MobInfo>("Data/Mobs"))
            {
                assetProviderService.Mobs.Add(mobInfo);
            }
            
            assetProviderService.Buffs = new List<BuffInfo>();
            
            foreach (BuffInfo buffInfo in Resources.LoadAll<BuffInfo>("Data/Buffs"))
            {
                assetProviderService.Buffs.Add(buffInfo);
            }
            
            assetProviderService.CharactersData = new List<CharacterData>();
            
            foreach (CharacterData characterData in Resources.LoadAll<CharacterData>("Data/Characters"))
            {
                assetProviderService.CharactersData.Add(characterData);
            }
            
            assetProviderService.Worlds = new List<World>();
            
            foreach (World world in Resources.LoadAll<World>("Data/Worlds"))
            {
                assetProviderService.Worlds.Add(world);
            }
            
            assetProviderService.BuffEffects = new List<BuffEffect>();
            
            foreach (BuffEffect buffEffect in Resources.LoadAll<BuffEffect>("BuffEffects"))
            {
                assetProviderService.BuffEffects.Add(buffEffect);
            }
            
            Debug.Log("Data Updated!");
        }
        
        public MobInfo GetMobInfo(MobEnum mobEnum)
        {
            foreach (var mob in Mobs)
            {
                if (mob.mobDefaultData.MobEnum == mobEnum)
                    return mob;
            }

            throw new Exception("Not found mob");
        }

        public BuffInfo GetBuffInfo(BuffEnum buffEnum)
        {
            foreach (var buff in Buffs)
            {
                if (buff.BuffData.BuffEnum == buffEnum)
                    return buff;
            }

            throw new Exception("Not found buff");
        }

        public CharacterData GetCharactersData(CharacterEnum characterEnum)
        {
            foreach (var character in CharactersData)
            {
                if (character.Character == characterEnum)
                    return character;
            }

            throw new Exception("Not found character");
        }
        
        public BuffEffect GetBuffEffect(EffectEnum effectEnum)
        {
            foreach (var buffEffect in BuffEffects)
            {
                if (buffEffect.EffectEnum == effectEnum)
                    return buffEffect;
            }

            throw new Exception("Not found BuffEffect");
        }
    }
}