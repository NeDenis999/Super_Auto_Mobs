using System;
using System.Collections;
using System.Security.Cryptography;
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

        private int _hearts = 0;
        private int _attack = 0;
        private int _armor = 0;
        private Perk _perk;
        private MobDefaultData _mobDefaultData;
        private int _currentHearts => _hearts + _mobDefaultData.Hearts;
        private int _currentAttack => _attack + _mobDefaultData.Attack;
        private bool _isEnemy;
        private MobData _mobData;
        private BuffEffect _buffEffect;

        public bool IsActive => _currentHearts > 0;
        public int CurrentHearts => _currentHearts;
        public int CurrentAttack => _currentAttack;
        public Perk Perk => _perk;
        public bool IsEnemy => _isEnemy;
        public MobData MobData => _mobData;
        public MobDefaultData MobDefaultData => _mobDefaultData;
        public Transform EffectPoint => _effectPoint;

        public void Init(MobDefaultData mobDefaultData, MobData mobData)
        {
            _mobDefaultData = mobDefaultData;
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

        public void SetBuffEffect(BuffEffect buffEffect)
        {
            if (_buffEffect)
            {
                _buffEffect.Deactivate(this);
                Destroy(_buffEffect.gameObject);
            }

            buffEffect.Activate(this);
            _mobData.EffectEnum = buffEffect.EffectEnum;
            _buffEffect = buffEffect;
        }
        
        public IEnumerator TakeDamage(int damage)
        {
            _hearts -= damage;
            OnChangeHearts?.Invoke(_currentHearts, -damage);
            _spriteRenderer.material = _assetProviderService.DamageMaterial;
            Invoke(nameof(EndAnimation), 0.2f);

            if (_currentHearts > 0 && _perk.TriggeringSituation == TriggeringSituation.TakeDamage)
            {
                StartCoroutine(_perk.Activate());
            }

            if (_currentHearts <= 0)
            {
                if (_perk.TriggeringSituation == TriggeringSituation.Faint)
                {
                    yield return _perk.Activate();
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
        
        public void ChangeArmour(int value)
        {
            _armor += value;
        }

        private void EndAnimation()
        {
            _spriteRenderer.material = _assetProviderService.DefaultMaterial;
        }
    }
}