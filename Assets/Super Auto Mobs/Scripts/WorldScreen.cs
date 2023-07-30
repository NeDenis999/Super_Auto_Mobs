using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class WorldScreen : MonoBehaviour
    {
        [SerializeField]
        private World _world;

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private Image _previewImage;
        
        [SerializeField]
        private Button _button;
        
        private SessionProgressService _sessionProgressService;
        private LanguageService _languageService;
        private Game _game;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService, LanguageService languageService, Game game)
        {
            _sessionProgressService = sessionProgressService;
            _languageService = languageService;
            _game = game;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OpenWorld);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OpenWorld);
        }

        private void Start()
        {
            _text.text = _languageService.GetText(_world.WorldData.Title);
            _previewImage.sprite = _world.Preview;
        }

        private void OpenWorld()
        {
            _sessionProgressService.SetWorldData(_world.WorldData);
            _game.CurrentGameState = GameState.Shop;
        }
    }
}