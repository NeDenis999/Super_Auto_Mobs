using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class InfoMobScreen : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _titleText, _infoText, _attackText, _heartsText;

        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        private bool _isOpen;
        private float _currentProgress;
        private float _speed = 2f;
        private LanguageService _languageService;

        [Inject]
        private void Construct(LanguageService languageService)
        {
            _languageService = languageService;
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

            if (entity is Mob)
            {
                var mob = (Mob)entity;
                
                _titleText.text = _languageService.GetText(mob.Name);
                _infoText.text = _languageService.GetText(mob.Info);
                _attackText.text = mob.CurrentAttack.ToString();
                _heartsText.text = mob.CurrentHearts.ToString();
            }
            
            if (entity is Buff)
            {
                var mob = (Buff)entity;
                
                _titleText.text = _languageService.GetText(mob.Name);
                _infoText.text = _languageService.GetText(mob.Info);
                _attackText.text = mob.CurrentAttack.ToString();
                _heartsText.text = mob.CurrentHearts.ToString();
            }
        }

        public void Close()
        {
            if (_isOpen)
                _isOpen = false;
        }
    }
}