using System.Collections;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class TutorialCutScene2 : CutScene
    {
        [SerializeField]
        private Dialogue _tutorialDialog4;
        
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
            _sessionProgressService.IsDisableRollButton = false;
            _sessionProgressService.IsDisableRollButton = false;
            _shopService.Open();
            yield return base.Play();
            yield return AwaitDialogHide(_tutorialDialog4);
        }
    }
}