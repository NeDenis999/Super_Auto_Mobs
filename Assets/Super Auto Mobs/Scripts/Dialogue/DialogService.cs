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
    public class DialogService : BaseWindow
    {
        public event Action OnHide;
        public event Action OnStartHide;

        public bool IsSkipDialogs;

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
        private GameObject _rightName;

        [SerializeField]
        private GameObject _leftName;

        [SerializeField]
        private TextMeshProUGUI _leftNameText;

        [SerializeField]
        private TextMeshProUGUI _rightNameText;

        [SerializeField]
        private Dialogue _dialogue;

        private ReplicaData _currentReplicaData => _dialogue.ReplicasData[_numberCurrentReplicaData];
        private int _numberCurrentReplica;
        private int _numberCurrentReplicaData;
        private bool _isShow;
        private LanguageService _languageService;
        private AssetProviderService _assetProviderService;
        private ShopService _shopService;

        [Inject]
        private void Construct(LanguageService languageService, AssetProviderService assetProviderService, ShopService shopService)
        {
            _shopService = shopService;
            _languageService = languageService;
            _assetProviderService = assetProviderService;
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
        
        public void Show(Dialogue dialogue = null)
        {
            if (IsSkipDialogs)
            {
                Hide();
                return;
            }
            
            _shopService.IsInteractive = false;
            
            if (dialogue == null)
                dialogue = _dialogue;

            _dialogue = dialogue;
            
            _dialogCanvas.SetActive(true);
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
                })
                .setOnComplete(() => _isShow = true);
            
            LeanTween.value(gameObject, 250, -250, 0.5f)
                .setOnUpdate(x =>
                {
                    rightPersonRect.anchoredPosition = rightPersonRect.anchoredPosition.SetX(x);
                });

            var isRightName = false;
            var isLeftName = false;
            
            foreach (var replicaData in dialogue.ReplicasData)
            {
                var characterData = _assetProviderService.GetCharactersData(replicaData.Character);
                
                if (!isRightName && replicaData.Side == SideEnum.Right)
                {
                    _rightNameText.text = _languageService.GetText(characterData.Name);
                    _rightPerson.GetComponent<Image>().sprite = characterData.View;
                    isRightName = true;
                }
                
                if (!isLeftName && replicaData.Side == SideEnum.Left)
                {
                    _leftNameText.text = _languageService.GetText(characterData.Name);
                    _leftPerson.GetComponent<Image>().sprite = characterData.View;
                    isLeftName = true;
                }

                if (isRightName && isLeftName)
                    break;
            }

            if (!isRightName)
            {
                _rightNameText.text = "";
                _rightPerson.GetComponent<Image>().sprite = null;
                _rightPerson.SetActive(false);
                _rightName.SetActive(false);
            }
            else
            {
                _rightPerson.SetActive(true);
                _rightName.SetActive(true);
            }
            
            if (!isLeftName)
            {
                _leftNameText.text = "";
                _leftPerson.GetComponent<Image>().sprite = null;
                _leftPerson.SetActive(false);
                _leftName.SetActive(false);
            }
            else
            {
                _rightPerson.SetActive(true);
                _leftName.SetActive(true);
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
                    OnHide?.Invoke();
                });
            
            _shopService.IsInteractive = true;
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
                _rightName.GetComponent<Image>().color = Color.white;
                _leftName.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
            else
            {
                _rightPerson.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                _leftPerson.GetComponent<Image>().color = Color.white;
                _rightName.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                _leftName.GetComponent<Image>().color = Color.white;
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

        private void NextReplicaData()
        {
            _numberCurrentReplicaData++;

            if (_numberCurrentReplicaData >= _dialogue.ReplicasData.Count)
            {
                Hide();
                return;
            }

            _numberCurrentReplica = 0;
            PrintReplica();
        }
    }
}