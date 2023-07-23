using System;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class Mob : Entity
    {
        public event Action<int, int> OnChangeHearts;
        public event Action<int, int> OnChangeAttack;
        public event Action<Mob> OnFaint;
        
        [SerializeField]
        private Transform _effectPoint;
        
        private int _hearts = 1;
        private int _attack = 1;
        private Perk _perk;
        private MobDefaultData mobDefaultData;
        private int _currentHearts => _hearts + mobDefaultData.Hearts;
        private int _currentAttack => _attack + mobDefaultData.Attack;
        private bool _isEnemy;
        private MobData _mobData;

        public bool IsActive => _currentHearts > 0;
        public int CurrentHearts => _currentHearts;
        public int CurrentAttack => _currentAttack;
        public Perk Perk => _perk;
        public bool IsEnemy => _isEnemy;
        public MobData MobData => _mobData;
        public Transform EffectPoint => _effectPoint;

        public void Init(MobDefaultData mobDefaultData, MobData mobData)
        {
            this.mobDefaultData = mobDefaultData;
            _hearts = mobDefaultData.Hearts;
            _attack = mobDefaultData.Attack;
            _name = mobDefaultData.Name;
            _info = mobDefaultData.Info;
            _mobData = mobData;
            
            _perk = GetComponent<Perk>();

            if (_perk == null)
                _perk = gameObject.AddComponent<AddCoinsPerk>();
        }
        
        public void Init(MobDefaultData mobDefaultData, MobData mobData, bool isEnemy)
        {
            Init(mobDefaultData, mobData);
            _isEnemy = isEnemy;
        }

        public void TakeDamage(int damage)
        {
            _hearts -= damage;
            OnChangeHearts?.Invoke(_currentHearts, -damage);
            _spriteRenderer.material = _assetProviderService.DamageMaterial;
            Invoke(nameof(EndAnimation), 1f);

            if (_currentHearts > 0 && _perk.TriggeringSituation == TriggeringSituation.TakeDamage)
            {
                StartCoroutine(_perk.Activate());
            }

            if (_currentHearts <= 0)
            {
                if (_perk.TriggeringSituation == TriggeringSituation.Faint)
                {
                    StartCoroutine(_perk.Activate());
                }
                
                OnFaint?.Invoke(this);
            }
        }

        public void ChangeHearts(int value)
        {
            _hearts += value;
            OnChangeHearts?.Invoke(_currentHearts, value);
        }
        
        public void ChangeAttack(int value)
        {
            _attack += value;
            OnChangeAttack?.Invoke(_currentAttack, value);
        }

        private void EndAnimation()
        {
            _spriteRenderer.material = _assetProviderService.DefaultMaterial;
        }
    }
}