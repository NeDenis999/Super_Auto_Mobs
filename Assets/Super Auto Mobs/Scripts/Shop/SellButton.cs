using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class SellButton : MonoBehaviour
    {
        private const float SizeY = 148;
        
        private Button _button;
        private ShopPlatformService shopPlatformService;
        private ShopService _shopService;

        private float _defaultPositionY;
        private RectTransform _rectTransform;

        [Inject]
        private void Construct(ShopPlatformService shopPlatformService, ShopService shopService)
        {
            this.shopPlatformService = shopPlatformService;
            _shopService = shopService;
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

        private void OnEnable()
        {
            shopPlatformService.OnSelectCommandPlatform += Show;
            shopPlatformService.OnUnselectCommandPlatform += Hide;
            _button.onClick.AddListener(Sell);
        }

        private void OnDisable()
        {
            shopPlatformService.OnSelectCommandPlatform -= Show;
            shopPlatformService.OnUnselectCommandPlatform -= Hide;
            _button.onClick.RemoveListener(Sell);
        }

        private void Show()
        {
            LeanTween.moveY(gameObject, _defaultPositionY, 0.5f);
        }

        private void Hide()
        {
            LeanTween.value(gameObject, _defaultPositionY, -SizeY, 0.5f)
                .setOnUpdate(y =>
                {
                    _rectTransform.anchoredPosition = _rectTransform.anchoredPosition.SetY(y);
                });
        }

        private void Sell()
        {
            shopPlatformService.DestroySelectEntity();
            _shopService.Sell();
        }
    }
}