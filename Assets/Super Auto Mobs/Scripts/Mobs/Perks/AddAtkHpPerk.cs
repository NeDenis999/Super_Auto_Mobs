using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class AddAtkHpPerk : Perk
    {
        private const float DelayAnimation = 1f;
        
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
        private int _currentHealth => _isPercent ? MathfExtensions.GetPercent(_mob.CurrentHearts * _health) : _health;

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
            else if (_game.CurrentGameState is GameState.Battle)
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

        private IEnumerator AddAtkHp(List<Mob> command, Mob myMob)
        {
            if (command.Count <= 1)
                yield break;
            
            var mobs = GetList(command, myMob);
            
            if (_currentAttack > 0)
                yield return AddAttack(mobs);
            
            if (_currentHealth > 0)
                yield return AddHearts(mobs);
        }

        private IEnumerator AddAtkHp(List<ShopCommandMobPlatform> command, ShopCommandMobPlatform myPlatform)
        {
            if (command.Count <= 1)
                yield break;
            
            var platforms = GetList(command, myPlatform);
            List<Mob> mobs = new List<Mob>();

            foreach (var platform in platforms)
            {
                mobs.Add(platform.Mob);
            }

            if (_currentAttack > 0)
                yield return AddAttack(mobs);
            
            if (_currentHealth > 0)
                yield return AddHearts(mobs);
        }

        private IEnumerator AddAttack(List<Mob> mobs)
        {
            foreach (var mob in mobs)
            {
                _sparkService.StartAnimation(_mob.EffectPoint.position, mob.EffectPoint.position,
                    SparkEnum.Attack, GetColor(_currentAttack), DelayAnimation);
            }
            
            yield return new WaitForSeconds(DelayAnimation);
            
            foreach (var mob in mobs)
            {
                mob.ChangeAttack(_currentAttack);
            } 
        }

        private IEnumerator AddHearts(List<Mob> mobs)
        {
            foreach (var mob in mobs)
            {
                _sparkService.StartAnimation(_mob.EffectPoint.position, mob.EffectPoint.position,
                    SparkEnum.Heart, GetColor(_currentAttack), DelayAnimation);
            }
            
            yield return new WaitForSeconds(DelayAnimation);
            
            foreach (var mob in mobs)
            {
                mob.ChangeHearts(_currentHealth);
            } 
        }

        private Color GetColor(int value)
        {
            var color = Color.white;

            if (value <= 0)
            {
                color = Color.black;
            }
            else if (value < 2)
            {
                color = Color.red;
            }
            else if (value < 5 + 1)
            {
                color = Color.yellow;
            }
            else if (value < 5 * 2 + 1)
            {
                color = Color.blue;
            }
            else if (value < 5 * 3 + 1)
            {
                color = Color.magenta;
            }
            else
            {
                color = Color.green;
            }

            return color;
        }

        private List<T> GetList<T>(List<T> list, T element)
        {
            if (list.Count == 0)
                return null;
            
            switch (_perkType)
            {
                case PerkTypeEnum.Random:
                    return RandomExtensions.GetRandomList(list.GetAllExcept(element), _mobsCount);
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