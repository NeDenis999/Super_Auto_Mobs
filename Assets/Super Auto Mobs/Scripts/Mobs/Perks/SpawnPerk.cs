using System.Collections;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class SpawnPerk : Perk
    {
        [SerializeField]
        private MobEnum _mobType;
        
        private BattleService _battleService;
        private Game _game;
        private AssetProviderService _assetProviderService;
        
        [Inject]
        private void Construct(BattleService battleBaseService, Game game, AssetProviderService assetProviderService)
        {
            _battleService = battleBaseService;
            _assetProviderService = assetProviderService;
            _game = game;
        }

        public override IEnumerator Activate()
        {
            print("SpawnPerk Activate");
            
            if (_game.CurrentGameState != GameState.Battle)
                Debug.LogError("Перк может быть вызван лишь во время боя");
            
            var mob = GetComponent<Mob>();

            var mobInfo = _assetProviderService.GetMobInfo(_mobType);
            
            var mobData = new MobData()
            {
                Attack = mobInfo.mobDefaultData.Attack,
                Hearts = mobInfo.mobDefaultData.Hearts,
                MobEnum = _mobType
            };
            
            _battleService.SpawnMob(mobData, mob.IsEnemy, true);
            yield return new WaitForSeconds(Constants.DelayPerk);
        }
    }
}