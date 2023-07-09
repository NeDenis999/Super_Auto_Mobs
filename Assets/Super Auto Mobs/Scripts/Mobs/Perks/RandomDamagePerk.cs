using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class RandomDamagePerk : Perk
    {
        [SerializeField]
        private int _mobsCount = 1;

        [SerializeField]
        private int _damage = 1;

        private BattleService _battleService;
        private Game _game;

        [Inject]
        private void Construct(BattleService battleService, Game game)
        {
            _battleService = battleService;
            _game = game;
        }
        
        public override void Activate()
        {
            if (_game.CurrentGameState != GameState.Battle)
                Debug.LogError("Перк может быть вызван лишь во время боя");
            
            var mob = GetComponent<Mob>();

            if (mob.IsEnemy)
            {
                TakeDamage(_battleService.MyCommandMobs);
            }
            else
            {
                TakeDamage(_battleService.EnemyCommandMobs);
            }
        }

        private void TakeDamage(List<Mob> command)
        {
            var enemies = RandomExtensions.GetRandomList(command, _mobsCount);
            
            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(_damage);
            }
        }
    }
}