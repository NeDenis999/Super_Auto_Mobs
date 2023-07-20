using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Super_Auto_Mobs
{
    public class ChangeCharacteristicsView : MonoBehaviour
    {
        private const int MaxCountIcons = 5;
        
        [SerializeField]
        private TextMeshProUGUI _heartsText, _attackText;

        [SerializeField]
        private GameObject _heartPrefab, _attackPrefab;
        
        [SerializeField]
        private Transform _heartCreatePoint, _attackCreatePoint;
        
        [SerializeField]
        private GameObject _heartChange, _attackChange;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Transform _transform;
        
        private Mob _mob;
        private List<GameObject> _hearts = new();
        private List<GameObject> _attacks = new();

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

        private void Start()
        {
            CreateHearts(_mob.CurrentHearts);
            ColorUpdate(_hearts, _mob.CurrentHearts);
            
            CreateAttacks(_mob.CurrentAttack);
            ColorUpdate(_attacks, _mob.CurrentAttack);
            
            if (_mob.IsEnemy)
            {
                transform.localScale = transform.localScale.SetX(-1); 
            }
        }

        private void OnDestroy()
        {
            LeanTween.cancelAll();
        }

        private void ChangeHearts(int value, int difference)
        {
            HeartsUpdate(value);
            ColorUpdate(_hearts, value);
            
            StartCoroutine(AwaitChangeHearts(value, difference));
        }

        private void ChangeAttack(int value, int difference)
        {
            AttacksUpdate(value);
            ColorUpdate(_attacks, value);
            
            StartCoroutine(AwaitChangeAttack(value, difference));
        }
        
        private void CreateHearts(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_hearts.Count < MaxCountIcons)
                {
                    _hearts.Add(Instantiate(_heartPrefab, _heartCreatePoint));
                }
            }
        }
        
        private void CreateAttacks(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_attacks.Count < MaxCountIcons)
                {
                    _attacks.Add(Instantiate(_attackPrefab, _attackCreatePoint));
                }
            }
        }
        
        private void HeartsUpdate(int value)
        {
            if (_hearts.Count < value)
            {
                CreateHearts(value - _hearts.Count);
            }
            else if (value < _hearts.Count)
            {
                for (int i = 0; i < _hearts.Count - value; i++)
                {
                    var heart = _hearts[i];
                    _hearts.Remove(heart);
                    Destroy(heart);
                }
            }
        }
        
        private void AttacksUpdate(int value)
        {
            if (value > _attacks.Count)
            {
                CreateAttacks(value - _attacks.Count);
            }
            else if (value < _attacks.Count)
            {
                for (int i = 0; i < _attacks.Count - value; i++)
                {
                    var attack = _attacks[i];
                    _attacks.Remove(attack);
                    Destroy(attack);
                }
            } 
        }
        
        private void ColorUpdate(List<GameObject> list, int value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var color = Color.white;

                if (value - i < MaxCountIcons + 1)
                {
                    color = Color.red;
                }
                else if (value - i < MaxCountIcons * 2 + 1)
                {
                    color = Color.yellow;
                }
                else if (value - i < MaxCountIcons * 3 + 1)
                {
                    color = Color.blue;
                }
                else if (value - i < MaxCountIcons * 4 + 1)
                {
                    color = Color.magenta;
                }
                else
                {
                    color = Color.green;
                }
                
                list[i].GetComponent<Image>().color = color;
            }
        }

        private IEnumerator AwaitChangeHearts(int value, int difference)
        {
            _heartChange.SetActive(true);
            
            _heartsText.text = difference > 0 ? "+" : "";
            _heartsText.text += $"{difference}";

            if (difference == 0)
                _heartsText.text = "MISS";

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
            _heartsText.text = difference > 0 ? "+" : "";
            _heartsText.text += $"{difference}";

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