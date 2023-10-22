using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class InfoMobScreen : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _titleText, _infoText, _attackText, _heartsText, _priceText;

        [SerializeField]
        private GameObject _price;
        
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        private bool _isOpen;
        private float _currentProgress;
        private float _speed = 2f;
        private LanguageService _languageService;
        private Game _game;


        [Inject]
        private void Construct(LanguageService languageService, Game game)
        {
            _languageService = languageService;
            _game = game;
        }

        private void Update()
        {
            if (_isOpen)
            {
                if (_currentProgress < 1f)
                    _currentProgress += Time.deltaTime * _speed;
            }
            else
            {
                if (_currentProgress > 0f)
                    _currentProgress -= Time.deltaTime * _speed;
            }

            _canvasGroup.alpha = Math.Clamp(_currentProgress, 0, 1);
        }

        public void Open(Entity entity, bool isNotChangePosition = false)
        {
            if (!_isOpen)
                _isOpen = true;
            
            if (!isNotChangePosition)
                transform.position = entity.transform.position.AddY(2.75f);

            _price.SetActive(_game.CurrentGameState == GameState.Shop);
            
            if (entity is Mob)
            {
                var mob = (Mob)entity;
                
                _titleText.text = _languageService.GetText(mob.Name);
                _infoText.text = _languageService.GetText(mob.Info);
                _attackText.text = mob.CurrentAttack.ToString();
                _heartsText.text = mob.CurrentHearts.ToString();
                _priceText.text = Constants.PriceEntity.ToString();
            }
            
            if (entity is Buff)
            {
                var buff = (Buff)entity;
                
                _titleText.text = _languageService.GetText(buff.Name);
                _infoText.text = _languageService.GetText(buff.Info);
                _attackText.text = buff.CurrentAttack.ToString();
                _heartsText.text = buff.CurrentHearts.ToString();
                _priceText.text = Constants.PriceEntity.ToString();
            }
        }

        public void Close()
        {
            if (_isOpen)
                _isOpen = false;
        }
    }
}