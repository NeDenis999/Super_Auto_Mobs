using System.Collections;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class Buff : Entity
    {
        public int Hearts;
        public int Attack;
        
        private const float SpeedAnimationMove = 8;

        public IEnumerator ToMoveTrajectory(Vector2[] trajectory)
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
            
            Destroy(gameObject);
        }
    }
}