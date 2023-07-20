using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class AddAtkHpPerk : Perk
    {
        [SerializeField]
        private bool _isPercent;
        
        [SerializeField]
        private int _atack = 1;
        
        [SerializeField]
        private int _health = 1;

        [SerializeField]
        private int _mobsCount = 1;

        [SerializeField]
        private PerkTypeEnum _perkType;

        private BattleService _battleService;
        private Game _game;
        private ShopService _shopService;
        private SparkService _sparkService;
        private Mob _mob;
        private int _currentAttack => _isPercent ? MathfExtensions.GetPercent(_mob.CurrentAttack * _atack) : _atack;

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
            print("AddAtkHpPerk Activate");

            if (_game.CurrentGameState == GameState.Shop)
            {
                var platform = GetComponentInParent<ShopCommandMobPlatform>();
                _mob = platform.Mob;
                
                yield return AddAtkHp(_shopService.CommandPlatforms, platform);
            }
            else if (_game.CurrentGameState is GameState.Battle or GameState.BattleTransition)
            {
                _mob = GetComponent<Mob>();

                if (!_mob.IsEnemy)
                {
                    yield return AddAtkHp(_battleService.MyCommandMobs, _mob);
                }
                else
                {
                    yield return AddAtkHp(_battleService.EnemyCommandMobs, _mob);
                }
            }
        }

        public IEnumerator AddAtkHp(List<Mob> command, Mob myMob)
        {
            if (command.Count <= 1)
                yield break;
            
            var mobs = GetList(command.GetAllExcept(myMob), myMob);

            foreach (var mob in mobs)
            {
                _sparkService.StartAnimation(transform.position, mob.transform.position);
            }

            yield return new WaitForSeconds(1);

            foreach (var mob in mobs)
            {
                mob.ChangeAttack(_currentAttack);
                mob.ChangeHearts(_health);
            }
        }
        
        public IEnumerator AddAtkHp(List<ShopCommandMobPlatform> command, ShopCommandMobPlatform myPlatform)
        {
            if (command.Count <= 1)
                yield break;
            
            var platforms = GetList(command.GetAllExcept(myPlatform), myPlatform);

            foreach (var platform in platforms)
            {
                var mob = platform.Mob;
                _sparkService.StartAnimation(transform.position, mob.transform.position);
            }
            
            yield return new WaitForSeconds(1);
            
            foreach (var platform in platforms)
            {
                var mob = platform.Mob;
                mob.ChangeAttack(_currentAttack);
                mob.ChangeHearts(_health);
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

    public enum PerkTypeEnum
    {
        Random,
        Forward,
        Back,
        All
    }
}