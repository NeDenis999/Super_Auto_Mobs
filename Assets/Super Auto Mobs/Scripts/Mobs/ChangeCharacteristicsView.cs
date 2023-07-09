using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class ChangeCharacteristicsView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _heartsText, _attackText;

        [SerializeField]
        private GameObject _heartChange, _attackChange;

        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        private Mob _mob;

        private void Awake()
        {
            _mob = GetComponentInParent<Mob>(false);
        }

        private void OnEnable()
        {
            _mob.OnChangeHearts += ChangeHearts;
            _mob.OnChangeAttack += ChangeAttack;
        }

        private void OnDisable()
        {
            _mob.OnChangeHearts -= ChangeHearts;
            _mob.OnChangeAttack -= ChangeAttack;
        }
        
        private void ChangeHearts(int value, int difference)
        {
            StartCoroutine(AwaitChangeHearts(value, difference));
        }
        
        private void ChangeAttack(int value, int difference)
        {
            StartCoroutine(AwaitChangeAttack(value, difference));
        }

        private IEnumerator AwaitChangeHearts(int value, int difference)
        {
            _heartChange.SetActive(true);
            _heartsText.text = $"+{difference}";

            LeanTween.value(0, 1, 0.3f)
                .setOnUpdate((x) => _canvasGroup.alpha = x);
            
            yield return new WaitForSeconds(0.5f);
            
            LeanTween.value(1, 0, 1f)
                .setOnUpdate((x) => _canvasGroup.alpha = x);
            
            _heartChange.LeanMoveLocalY(_heartChange.transform.localPosition.y + 0.1f, 0.5f);
            yield return new WaitForSeconds(1f);
            _heartChange.SetActive(false);
        }
        
        private IEnumerator AwaitChangeAttack(int value, int difference)
        {
            _attackChange.SetActive(true);
            _attackText.text = $"+{difference}";

            LeanTween.value(0, 1, 0.3f)
                .setOnUpdate((x) => _canvasGroup.alpha = x);
            
            yield return new WaitForSeconds(0.5f);
            
            LeanTween.value(1, 0, 1f)
                .setOnUpdate((x) => _canvasGroup.alpha = x);
            
            _attackChange.LeanMoveLocalY(_attackChange.transform.localPosition.y + 0.1f, 0.5f);
            yield return new WaitForSeconds(1f);
            _attackChange.SetActive(false);
        }
    }
}