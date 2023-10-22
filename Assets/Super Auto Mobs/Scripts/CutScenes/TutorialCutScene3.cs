using System.Collections;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class TutorialCutScene3 : CutScene
    {
        [SerializeField]
        private Dialogue _tutorialDialog5;
        
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
            _shopService.Open();
            yield return base.Play();
            yield return AwaitDialogHide(_tutorialDialog5);

            _sessionProgressService.IsEndData = true;
        }
    }
}