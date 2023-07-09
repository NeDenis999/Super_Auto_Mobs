using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        
        private LoadScreenService _loadScreenService;
        private Game _game;

        [Inject]
        private void Construct(Game game, LoadScreenService loadScreenService)
        {
            _game = game;
            _loadScreenService = loadScreenService;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(StartBattle);
            
            _loadScreenService.OnClose += () => _loadScreenService.StartCoroutine(CloseLoadScreen());
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(StartBattle);
            
            _loadScreenService.OnClose -= () => _loadScreenService.StartCoroutine(CloseLoadScreen());
        }

        private void StartBattle()
        {
            _loadScreenService.Close();
            _game.CurrentGameState = GameState.ShopTransition;
        }
        
        private IEnumerator CloseLoadScreen()
        {
            yield return new WaitForSeconds(0.2f);
            _loadScreenService.Open();
            _game.CurrentGameState = GameState.Shop;
        }
    }
}