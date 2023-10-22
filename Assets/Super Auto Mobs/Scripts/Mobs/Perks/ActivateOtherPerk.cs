using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class ActivateOtherPerk : Perk
    {
        [SerializeField]
        private PerkTypeEnum _perkType;
        
        [SerializeField]
        private int _mobsCount = 1;
        
        private BattleService _battleService;
        private Game _game;
        private ShopService _shopService;
        private SparkService _sparkService;
        
        [Inject]
        private void Construct(BattleService battleService, Game game, ShopService shopService, SparkService sparkService)
        {
            _battleService = battleService;
            _game = game;
            _shopService = shopService;
            _sparkService = sparkService;
        }
        
        public override IEnumerator Activate()
        {
            print("ActivateOtherPerk Activate");

            if (_game.CurrentGameState == GameState.Shop)
            {
                var platform = GetComponentInParent<ShopCommandMobPlatform>();

                yield return ActivateOthersPerk(_shopService.CommandPlatforms, platform);
            }
            else if (_game.CurrentGameState is GameState.Battle)
            {
                var mob = GetComponent<Mob>();

                if (!mob.IsEnemy)
                {
                    yield return ActivateOthersPerk(_battleService.MyCommandMobs, mob);
                }
                else
                {
                    yield return ActivateOthersPerk(_battleService.EnemyCommandMobs, mob);
                }
            }
        }
        
        public IEnumerator ActivateOthersPerk(List<Mob> command, Mob myMob)
        {
            if (command.Count <= 1)
                yield break;
            
            var mobs = GetList(command.GetAllExcept(myMob), myMob);

            foreach (var mob in mobs)
            {
                _sparkService.StartAnimation(transform.position, mob.transform.position, 
                    SparkEnum.ActivatePerk);
            }

            yield return new WaitForSeconds(Constants.DelayPerk);

            foreach (var mob in mobs)
            {
                StartCoroutine(mob.Perk.Activate());
            }
        }
        
        public IEnumerator ActivateOthersPerk(List<ShopCommandMobPlatform> command, ShopCommandMobPlatform myPlatform)
        {
            if (command.Count <= 1)
                yield break;
            
            var platforms = GetList(command.GetAllExcept(myPlatform), myPlatform);

            foreach (var platform in platforms)
            {
                var mob = platform.Mob;
                _sparkService.StartAnimation(transform.position, mob.transform.position, 
                    SparkEnum.ActivatePerk);
            }
            
            yield return new WaitForSeconds(Constants.DelayPerk);
            
            foreach (var platform in platforms)
            {
                var mob = platform.Mob;
                StartCoroutine(mob.Perk.Activate());
            }
        }

        public List<T> GetList<T>(List<T> list, T element)
        {
            if (list.Count == 0)
                return null;
            
            switch (_perkType)
            {
                case PerkTypeEnum.Random:
                    return RandomExtensions.GetRandomList(list, _mobsCount);
                case PerkTypeEnum.Forward:
                    return list.GetElementsAfterIndex(list.IndexOf(element));
                case PerkTypeEnum.Back:
                    return list.GetElementsBeforeIndex(list.IndexOf(element));
                case PerkTypeEnum.All:
                    if (list.Count > 1)
                        return list.GetAllExcept(element);
                    else
                        return list;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}