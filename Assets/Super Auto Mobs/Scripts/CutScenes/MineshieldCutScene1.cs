using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Super_Auto_Mobs
{
    public class MineshieldCutScene1 : CutScene
    {
        public UnityEvent OnHideDialog1;
        
        [SerializeField]
        private Dialogue _dialog1;
        
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
            _dialogService.Show(_dialog1);
            _dialogService.OnHide += HideDialog1;
        }
        
        private void HideDialog1()
        {
            _dialogService.OnHide -= HideDialog1;
            OnHideDialog1.Invoke();
        }
    }
}