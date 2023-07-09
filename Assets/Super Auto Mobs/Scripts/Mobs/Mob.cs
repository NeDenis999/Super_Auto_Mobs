using System;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Mob : Entity
    {
        public event Action<int, int> OnChangeHearts;
        public event Action<int, int> OnChangeAttack;
        
        [SerializeField]
        private int _hearts = 1;

        [SerializeField]
        private int _attack = 1;

        private Perk _perk;
        private MobData _mobData;
        private int _currentHearts;
        private int _currentAttack;
        private bool _isEnemy;

        public bool IsActive => _currentHearts > 0;
        public int CurrentHearts => _currentHearts;
        public int CurrentAttack => _currentAttack;
        public Perk Perk => _perk;
        public bool IsEnemy => _isEnemy;

        public void Init(MobData mobData)
        {
            _mobData = mobData;
            _currentHearts = _hearts + mobData.Hearts;
            _currentAttack = _attack + mobData.Attack;
            
            _perk = GetComponent<Perk>();

            if (_perk == null)
                _perk = gameObject.AddComponent<AddCoinsPerk>();
        }
        
        public void Init(MobData mobData, bool isEnemy)
        {
            Init(mobData);
            _isEnemy = isEnemy;
        }

        public void TakeDamage(int damage)
        {
            _currentHearts -= damage;
            
            if (_currentHearts > 0 && _perk.TriggeringSituation == TriggeringSituation.TakeDamage)
            {
                _perk.Activate();
            }

            if (_currentHearts <= 0 && _perk.TriggeringSituation == TriggeringSituation.Faint)
            {
                _perk.Activate();
            }
        }

        public void ChangeHearts(int value)
        {
            _mobData.Hearts += value;
            _currentHearts += value;
            OnChangeHearts?.Invoke(_mobData.Hearts, value);
        }
        
        public void ChangeAttack(int value)
        {
            _mobData.Attack += value;
            _currentAttack += value;
            OnChangeAttack?.Invoke(_mobData.Attack, value);
        }
    }
}