using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Zenject;


namespace Super_Auto_Mobs
{
    public class TutorialCutScene1 : CutScene
    {
        [SerializeField]
        private Dialogue _tutorialDialog1, _tutorialDialog2, _tutorialDialog3;

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
            yield return AwaitDialogHide(_tutorialDialog1);
            
            _shopUpdaterService.SpawnMob(MobEnum.Chicken);
            yield return AwaitBuy();
            
            yield return AwaitDialogHide(_tutorialDialog2);
            _shopUpdaterService.SpawnBuff(BuffEnum.Tag);
            _shopUpdaterService.SpawnBuff(BuffEnum.Apple);
            
            yield return AwaitBuy();
            yield return AwaitDialogHide(_tutorialDialog3);
            _shopService.EnableBattleButton();
        }

        private IEnumerator AwaitBuy(Action method = null)
        {
            var trigger = false;
            Action<PurchaseEnum> action = (purchaseEnum) => trigger = true;
            _shopTradeService.OnBuy += action;
            yield return new WaitUntil(() => trigger);
            _shopTradeService.OnBuy -= action;
            method?.Invoke();
        }
    }
}