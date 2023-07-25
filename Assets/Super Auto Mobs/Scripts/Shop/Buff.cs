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

        public int CurrentAttack => _attack;
        public int CurrentHearts => _hearts;

        public void Init(BuffData buffData)
        {
            _hearts = buffData.Hearts;
            _attack = buffData.Attack;
        }
        
        public IEnumerator ToMoveTrajectory(Vector2[] trajectory, Mob mob)
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
                        transform.position, target, Time.deltaTime * SpeedAnimationMove * previousDistance);
                    return (Vector2)transform.position == target;
                });
            }
            
            _soundsService.PlayEat();
            
            if (_hearts != 0)
                mob.ChangeHearts(_hearts);
            
            if (_attack != 0)
                mob.ChangeAttack(_attack);
            
            if (mob.Perk.TriggeringSituation == TriggeringSituation.Eat)
            {
                StartCoroutine(mob.Perk.Activate());
            }
            
            Destroy(gameObject);
        }
    }
}