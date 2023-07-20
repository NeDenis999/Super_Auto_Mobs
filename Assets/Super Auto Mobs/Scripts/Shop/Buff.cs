using System.Collections;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class Buff : Entity
    {
        public int Hearts;
        public int Attack;
        
        private const float SpeedAnimationMove = 8;

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
            
            if (Hearts != 0)
                mob.ChangeHearts(Hearts);
            
            if (Attack != 0)
                mob.ChangeAttack(Attack);
            
            if (mob.Perk.TriggeringSituation == TriggeringSituation.Eat)
            {
                StartCoroutine(mob.Perk.Activate());
            }
            
            Destroy(gameObject);
        }
    }
}