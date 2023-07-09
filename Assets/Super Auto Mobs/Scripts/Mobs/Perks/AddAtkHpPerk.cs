using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class AddAtkHpPerk : Perk
    {
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
        private Shop _shopService;

        [Inject]
        private void Construct(BattleService battleService, Game game, Shop shopService)
        {
            _battleService = battleService;
            _game = game;
            _shopService = shopService;
        }
        
        public override void Activate()
        {
            print("AddAtkHpPerk Activate");

            if (_game.CurrentGameState == GameState.Shop)
            {
                var platform = GetComponentInParent<ShopCommandMobPlatform>();
                
                AddAtkHp(_shopService.CommandPlatforms, platform);
            }
            else if (_game.CurrentGameState is GameState.Battle or GameState.BattleTransition)
            {
                var mob = GetComponent<Mob>();

                if (!mob.IsEnemy)
                {
                    AddAtkHp(_battleService.MyCommandMobs, mob);
                }
                else
                {
                    AddAtkHp(_battleService.EnemyCommandMobs, mob);
                }
            }
        }

        public void AddAtkHp(List<Mob> command, Mob myMob)
        {
            if (command.Count <= 1)
                return;
            
            var mobs = GetList(command.GetAllExcept(myMob), myMob);

            foreach (var mob in mobs)
            {
                mob.ChangeAttack(_atack);
                mob.ChangeHearts(_health);
            }
        }
        
        public void AddAtkHp(List<ShopCommandMobPlatform> command, ShopCommandMobPlatform myPlatform)
        {
            var platforms = GetList(command, myPlatform);

            foreach (var platform in platforms)
            {
                var mob = platform.Mob;
                mob.ChangeAttack(_atack);
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
                    return list.GetAllExcept(element);
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