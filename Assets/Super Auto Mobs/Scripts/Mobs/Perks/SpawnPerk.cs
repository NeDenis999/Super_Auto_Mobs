using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class SpawnPerk : Perk
    {
        [SerializeField]
        private MobData _spawnMobData;
        
        private BattleService _battleService;
        private Game _game;

        [Inject]
        private void Construct(BattleService battleBaseService, Game game)
        {
            this._battleService = battleBaseService;
            _game = game;
        }
        
        public override void Activate()
        {
            print("SpawnPerk Activate");
            
            if (_game.CurrentGameState != GameState.Battle)
                Debug.LogError("Перк может быть вызван лишь во время боя");
            
            var mob = GetComponent<Mob>();
            _battleService.SpawnMob(_spawnMobData, mob.IsEnemy);
        }
    }
}