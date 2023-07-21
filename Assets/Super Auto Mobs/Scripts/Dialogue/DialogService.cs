using System;
using System.Collections;
using System.Collections.Generic;
using I2.TextAnimation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class DialogService : MonoBehaviour
    {
        public event Action OnHide;
        public event Action OnStartHide;
        
        [SerializeField]
        private TextMeshProUGUI _textMeshPro;
        
        [SerializeField]
        private TextAnimation _textAnimation;
        
        [SerializeField]
        private GameObject _dialogCanvas;
        
        [SerializeField]
        private GameObject _rightPerson;
        
        [SerializeField]
        private GameObject _leftPerson;

        [SerializeField]
        private TextMeshProUGUI _leftNameText;
        
        [SerializeField]
        private TextMeshProUGUI _rightNameText;
        
        [SerializeField]
        private GameObject _blackout;
        
        [SerializeField]
        private List<Sprite> _viewsPerson;

        [SerializeField]
        private List<ReplicaData> _replicasData;

        private ReplicaData _currentReplicaData => _replicasData[_numberCurrentReplicaData];
        private int _numberCurrentReplica;
        private int _numberCurrentReplicaData;
        private bool _isShow;
        private LanguageService _languageService;

        [Inject]
        private void Construct(LanguageService languageService)
        {
            _languageService = languageService;
        }
        
        private void Update()
        {
            if (!_isShow)
                return;
            
            if (Input.GetMouseButtonUp(0))
            {
                NextReplica();
            }
        }
        
        public void Show(List<ReplicaData> replicasData = null)
        {
            if (replicasData == null)
                replicasData = _replicasData;

            _replicasData = replicasData;
            
            _isShow = true;
            _dialogCanvas.SetActive(true);
            _blackout.SetActive(true);
            _textMeshPro.text = "";
            _rightPerson.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            _leftPerson.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            
            var leftPersonRect = _leftPerson.GetComponent<RectTransform>();
            leftPersonRect.anchoredPosition = leftPersonRect.anchoredPosition.SetX(0);
            var rightPersonRect = _rightPerson.GetComponent<RectTransform>();
            rightPersonRect.anchoredPosition = rightPersonRect.anchoredPosition.SetX(0);

            LeanTween.value(gameObject, -250, 250, 0.5f)
                .setOnUpdate(x =>
                {
                    leftPersonRect.anchoredPosition = leftPersonRect.anchoredPosition.SetX(x);
                });
            
            LeanTween.value(gameObject, 250, -250, 0.5f)
                .setOnUpdate(x =>
                {
                    rightPersonRect.anchoredPosition = rightPersonRect.anchoredPosition.SetX(x);
                });

            foreach (var replicaData in replicasData)
            {
                var isRightName = false;
                var isLeftName = false;

                if (!isRightName && replicaData.Side == SideEnum.Right)
                {
                    _rightNameText.text = GetNamePerson(replicaData.Character);
                    _rightPerson.GetComponent<Image>().sprite = GetViewPerson(replicaData.Character);
                    isRightName = true;
                }
                
                if (!isLeftName && replicaData.Side == SideEnum.Left)
                {
                    _leftNameText.text = GetNamePerson(replicaData.Character);
                    _leftPerson.GetComponent<Image>().sprite = GetViewPerson(replicaData.Character);
                    isLeftName = true;
                }

                if (isRightName && isLeftName)
                    break;
            }

            StartCoroutine(AwaitPrintReplica());
        }
        
        public void Hide()
        {
            OnStartHide?.Invoke();
            
            _isShow = false;
            _numberCurrentReplica = 0;
            _numberCurrentReplicaData = 0;
            
            var leftPersonRect = _leftPerson.GetComponent<RectTransform>();
            leftPersonRect.anchoredPosition = leftPersonRect.anchoredPosition.SetX(0);
            var rightPersonRect = _rightPerson.GetComponent<RectTransform>();
            rightPersonRect.anchoredPosition = rightPersonRect.anchoredPosition.SetX(0);

            _textMeshPro.text = "";
            _rightPerson.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            _leftPerson.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            
            LeanTween.value(gameObject, leftPersonRect.anchoredPosition.x, -250, 0.5f)
                .setOnUpdate(x =>
                {
                    leftPersonRect.anchoredPosition = leftPersonRect.anchoredPosition.SetX(x);
                });
            
            LeanTween.value(gameObject, rightPersonRect.anchoredPosition.x, 250, 0.5f)
                .setOnUpdate(x =>
                {
                    rightPersonRect.anchoredPosition = rightPersonRect.anchoredPosition.SetX(x);
                })
                .setOnComplete(() =>
                {
                    _dialogCanvas.SetActive(false);
                    _blackout.SetActive(false);
                    OnHide?.Invoke();
                });
        }

        public void NextReplica()
        {
            _numberCurrentReplica++;

            if (_numberCurrentReplica >= _currentReplicaData.Replaces.Count)
            {
                NextReplicaData();
                return;
            }

            PrintReplica();
        }

        public void PrintReplica()
        {
            if (_currentReplicaData.Side == SideEnum.Right)
            {
                _rightPerson.GetComponent<Image>().color = Color.white;
                _leftPerson.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
            else
            {
                _rightPerson.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                _leftPerson.GetComponent<Image>().color = Color.white;
            }

            _textAnimation.StopAllAnimations();
            _textAnimation.PlayAnim();
            _textMeshPro.text = _languageService.GetText(_currentReplicaData.Replaces[_numberCurrentReplica]);
        }
        
        private IEnumerator AwaitPrintReplica()
        {
            yield return new WaitForSeconds(0.5f);
            PrintReplica();
        }

        private string GetNamePerson(CharacterEnum character)
        {
            if (character == CharacterEnum.Palesos)
                return "Palesos";
            
            if (character == CharacterEnum.CaptainEugene)
                return "CaptainEugene";

            return "Add Name";
        }
        
        private Sprite GetViewPerson(CharacterEnum character)
        {
            if (character == CharacterEnum.Palesos)
                return _viewsPerson[0];
            
            if (character == CharacterEnum.CaptainEugene)
                return _viewsPerson[1];

            return null;
        }

        private void NextReplicaData()
        {
            _numberCurrentReplicaData++;

            if (_numberCurrentReplicaData >= _replicasData.Count)
            {
                Hide();
                return;
            }

            _numberCurrentReplica = 0;
            PrintReplica();
        }
    }
}