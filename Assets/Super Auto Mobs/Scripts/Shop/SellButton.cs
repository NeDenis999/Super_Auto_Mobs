using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class SellButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private const float SizeY = 148;
        
        private Button _button;
        private ShopService shopService;
        private ShopTradeService shopTradeService;

        private float _defaultPositionY;
        private RectTransform _rectTransform;
        private bool _isEnter;

        [Inject]
        private void Construct(ShopService shopService, ShopTradeService shopTradeService)
        {
            this.shopService = shopService;
            this.shopTradeService = shopTradeService;
        }

        private void Awake()
        {
            _defaultPositionY = transform.position.y;
            _rectTransform = GetComponent<RectTransform>();
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _rectTransform.anchoredPosition = _rectTransform.anchoredPosition.SetY(-SizeY);
        }
        
        private void TrySell(PlatformServiceState platformServiceState)
        {
            if (_isEnter && platformServiceState == PlatformServiceState.NoChoosePlatform && shopService.ShopPlatformSelected)
            {
                Sell();
                _isEnter = false;
            }
        }
        
        private void OnEnable()
        {
            shopService.OnSelectCommandPlatform += Show;
            shopService.OnUnselectCommandPlatform += Hide;
            shopService.OnUpdateState += TrySell;
            _button.onClick.AddListener(Sell);
        }

        private void OnDisable()
        {
            shopService.OnSelectCommandPlatform -= Show;
            shopService.OnUnselectCommandPlatform -= Hide;
            shopService.OnUpdateState -= TrySell;
            _button.onClick.RemoveListener(Sell);
        }

        private void Show()
        {
            if (shopService.IsDisableSellButton)
                return;
            
            LeanTween.value(gameObject, -SizeY, _defaultPositionY, 0.25f)
                .setOnUpdate(y =>
                {
                    _rectTransform.anchoredPosition = _rectTransform.anchoredPosition.SetY(y);
                });
        }

        private void Hide()
        {
            LeanTween.value(gameObject, _defaultPositionY, -SizeY, 0.25f)
                .setOnUpdate(y =>
                {
                    _rectTransform.anchoredPosition = _rectTransform.anchoredPosition.SetY(y);
                });
        }

        private void Sell()
        {
            print("Sell");
            var perk = ((Mob)(shopService.ShopPlatformSelected.Entity)).Perk;
            
            if (perk.TriggeringSituation == TriggeringSituation.Sell)
            {
                StartCoroutine(perk.Activate());
            }
            
            shopService.DestroySelectEntity();
            shopTradeService.Sell();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            print("OnPointerEnter");
            _isEnter = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            print("OnPointerExit");
            _isEnter = false;
        }
    }
}