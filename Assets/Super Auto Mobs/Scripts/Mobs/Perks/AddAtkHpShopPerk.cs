using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class AddAtkHpShopPerk : Perk
    {
        [SerializeField]
        private int _atack = 1;
        
        [SerializeField]
        private int _health = 1;

        [SerializeField]
        private int _mobsCount = 1;
        
        private ShopUpdaterService _shopUpdaterService;

        [Inject]
        private void Construct(ShopUpdaterService shopUpdaterService)
        {
            _shopUpdaterService = shopUpdaterService;
        }
        
        public override void Activate()
        {
            print("AddAtkHpShopPerk Activate");
            var platforms = RandomExtensions.GetRandomList(_shopUpdaterService.ShopMobPlatforms, _mobsCount);

            foreach (var platform in platforms)
            {
                var mob = (Mob)platform.Entity;
                mob.ChangeAttack(_atack);
                mob.ChangeHearts(_health);
            }
        }
    }
}