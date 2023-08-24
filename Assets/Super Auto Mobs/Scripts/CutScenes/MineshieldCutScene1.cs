using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Super_Auto_Mobs
{
    public class MineshieldCutScene1 : CutScene
    {
        [SerializeField]
        private Dialogue _dialog1;

        private ShopUpdaterService _shopUpdaterService;
        private ShopService _shopService;
        private ShopTradeService _shopTradeService;
        private Game _game;
        
        [Inject]
        private void Construct(ShopUpdaterService shopUpdaterService,
            AssetProviderService assetProviderService, ShopService shopService,
            ShopTradeService shopTradeService, Game game)
        {
            _shopUpdaterService = shopUpdaterService;
            _shopService = shopService;
            _shopTradeService = shopTradeService;
            _game = game;
        }
        
        public override IEnumerator Play()
        {
            yield return base.Play();
            yield return AwaitDialogHide(_dialog1);
        }
    }
}