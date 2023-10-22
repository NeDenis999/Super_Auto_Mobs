using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _textMeshProUGUI;
        
        private Game _game;
        private CanvasGroup _canvasGroup;
        private SessionProgressService _sessionProgressService;
        private LanguageService _languageService;

        [Inject]
        private void Construct(Game game, SessionProgressService sessionProgressService, LanguageService languageService)
        {
            _game = game;
            _sessionProgressService = sessionProgressService;
            _languageService = languageService;
        }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            CheckGameState(_game.CurrentGameState);
            
            _game.OnUpdateGameState += CheckGameState;
        }

        private void OnDestroy()
        {
            _game.OnUpdateGameState -= CheckGameState;
        }

        private void CheckGameState(GameState gameState)
        {
            if (gameState == GameState.Battle)
            {
                Close();
            }
            else if (gameState == GameState.Shop)
            {
                Open();
            }
        }

        private void Open()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            _textMeshProUGUI.text = _languageService.GetText(_sessionProgressService.CurrentWorldData.Title);
        }

        private void Close()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}