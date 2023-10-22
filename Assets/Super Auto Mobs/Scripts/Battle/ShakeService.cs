using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class ShakeService : MonoBehaviour 
    {
        private const float FrequencyUpdates = 0.1f;

        public float shakeMagnitude = 0.5f;

        public IEnumerator Shake(GameObject shakeObject, float duration)
        {
            var initialPosition = shakeObject.transform.localPosition;

            while (duration > 0)
            {
                LeanTween.moveLocal(shakeObject, initialPosition + Random.insideUnitSphere * shakeMagnitude, FrequencyUpdates);
                duration -= FrequencyUpdates;
                yield return new WaitForSeconds(FrequencyUpdates);
            }
            
            LeanTween.moveLocal(shakeObject, initialPosition, FrequencyUpdates);
            yield return new WaitForSeconds(FrequencyUpdates);
        }
    }
}