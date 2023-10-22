using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class RandomDamagePerk : Perk
    {
        [SerializeField]
        private bool _isPercent;

        [SerializeField]
        private bool _isAll;
        
        [SerializeField]
        private int _mobsCount = 1;

        [SerializeField]
        private int _damage = 1;

        private BattleService _battleService;
        private Game _game;
        private SparkService _sparkService;
        private Mob _mob;

        [Inject]
        private void Construct(BattleService battleService, Game game, SparkService sparkService)
        {
            _battleService = battleService;
            _game = game;
            _sparkService = sparkService;
        }
        
        public override IEnumerator Activate()
        {
            if (_game.CurrentGameState != GameState.Battle)
                Debug.LogError("Перк может быть вызван лишь во время боя");
            
            _mob = GetComponent<Mob>();

            if (_mob.IsEnemy)
            {
                yield return TakeDamage(_battleService.MyCommandMobs);
            }
            else
            {
                yield return TakeDamage(_battleService.EnemyCommandMobs);
            }
        }

        private IEnumerator TakeDamage(List<Mob> command)
        {
            var enemies = _isAll ? command : RandomExtensions.GetRandomList(command, _mobsCount);
            var damage = _isPercent ? _mob.CurrentAttack * _damage / 100 : _damage;
            
            foreach (var enemy in enemies)
            {
                _sparkService.StartAnimation(transform.position, enemy.transform.position, SparkEnum.Damage);
            }

            yield return new WaitForSeconds(Constants.DelayPerk);
            
            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}