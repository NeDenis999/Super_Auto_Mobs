using System.Collections;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class TutorialCutScene : CutScene
    {
        [SerializeField]
        private Dialogue _tutorialDialog1, _tutorialDialog2, _tutorialDialog3, _tutorialDialog4;

        private ShopUpdaterService _shopUpdaterService;
        private ShopService _shopService;
        private DialogService _dialogService;
        private ShopTradeService _shopTradeService;
        private Game _game;

        [Inject]
        private void Construct(ShopUpdaterService shopUpdaterService,
            AssetProviderService assetProviderService, ShopService shopService, DialogService dialogService,
            ShopTradeService shopTradeService, Game game)
        {
            _shopUpdaterService = shopUpdaterService;
            _shopService = shopService;
            _dialogService = dialogService;
            _shopTradeService = shopTradeService;
            _game = game;
        }
        
        public override void Play()
        {
            base.Play();
            _dialogService.Show(_tutorialDialog1);
            _dialogService.OnHide += HideDialog1;
            _shopTradeService.OnBuy += Act2;
        }

        private void HideDialog1()
        {
            _dialogService.OnHide -= HideDialog1;
            _shopUpdaterService.SpawnMob(MobEnum.Chicken);
        }
        
        private void Act2(PurchaseEnum purchaseEnum)
        {
            if (purchaseEnum != PurchaseEnum.Mob)
                return;
            
            _shopTradeService.OnBuy -= Act2;
            _dialogService.OnHide += HideDialog2;
            _dialogService.Show(_tutorialDialog2);
        }

        private void HideDialog2()
        {
            _dialogService.OnHide -= HideDialog2;
            _shopTradeService.OnBuy += Act3;
            _shopUpdaterService.SpawnBuff(BuffEnum.Tag);
            _shopUpdaterService.SpawnBuff(BuffEnum.Apple);
        }
        
        private void Act3(PurchaseEnum purchaseEnum)
        {
            _shopTradeService.OnBuy -= Act3; 
            _dialogService.Show(_tutorialDialog3);
            _dialogService.OnHide += HideDialog3;
        }

        private void HideDialog3()
        {
            _dialogService.OnHide -= HideDialog3;
            _game.OnUpdateGameState += Act4;
            _shopService.EnableBattleButton();
        }

        private void Act4(GameState gameState)
        {
            if (gameState != GameState.Shop)
                return;
            
            _game.OnUpdateGameState -= Act4;
            _dialogService.Show(_tutorialDialog4);
            _dialogService.OnHide += HideDialog4;
        }

        private void HideDialog4()
        {
            _dialogService.OnHide -= HideDialog4;
            _sessionProgressService.IsEndData = true;
        }
    }
}