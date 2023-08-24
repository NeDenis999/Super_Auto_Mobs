using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Buff : Entity
    {
        private const float SpeedAnimationMove = 8;
        
        private int _hearts;
        private int _attack;
        private BuffData _buffData;
        private MobFactoryService _mobFactoryService;

        public int CurrentAttack => _attack;
        public int CurrentHearts => _hearts;
        public BuffData BuffData => _buffData;

        [Inject]
        private void Construct(MobFactoryService mobFactoryService)
        {
            _mobFactoryService = mobFactoryService;
        }
        
        public void Init(BuffData buffData)
        {
            _hearts = buffData.Hearts;
            _attack = buffData.Attack;
            _name = buffData.Name;
            _info = buffData.Info;
            _buffData = buffData;
        }
        
        public IEnumerator ToMoveTrajectory(Vector2[] trajectory, Mob mob, Action onEndMoveAnimation)
        {
            LTSeq sequence = LeanTween.sequence();
            var previousDistance = 1f;
            var previousTarget = Vector2.zero;
            Unselect();
            
            foreach (var target in trajectory)
            {
                if (previousTarget.magnitude != 0)
                    previousDistance = Vector2.Distance(previousTarget, target);
                previousTarget = target;
                
                yield return new WaitUntil(() => {
                    transform.position = Vector2.MoveTowards(
                        transform.position, target, Time.deltaTime * SpeedAnimationMove 
                                                                   * previousDistance);
                    return (Vector2)transform.position == target;
                });
            }
            
            _soundsService.PlayEat(_buffData.BuffSoundEnum);
            
            if (_hearts != 0)
                mob.ChangeHearts(_hearts);
            
            if (_attack != 0)
                mob.ChangeAttack(_attack);
            
            if (mob.Perk.TriggeringSituation == TriggeringSituation.Eat)
            {
                StartCoroutine(mob.Perk.Activate());
            }

            onEndMoveAnimation?.Invoke();
            
            if (BuffData.EffectEnum != EffectEnum.None)
            {
                _mobFactoryService.CreateBuffEffect(BuffData.EffectEnum, mob);
            }
            
            Destroy(gameObject);
        }
    }
}