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
            _button.onClick.AddListener(CloseBattle);
            _loadScreenService.OnClose += CloseLoadScreen;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(CloseBattle);
            _loadScreenService.OnClose -= CloseLoadScreen;
        }

        private void CloseBattle()
        {
            _loadScreenService.Close();
            _button.onClick.RemoveListener(CloseBattle);
        }
        
        private IEnumerator AwaitCloseLoadScreen()
        {
            _game.CurrentGameState = GameState.ShopTransition;
            yield return new WaitForSeconds(0.2f);
            _loadScreenService.Open();
            _game.CurrentGameState = GameState.Shop;
        }

        private void CloseLoadScreen()
        {
            _loadScreenService.OnClose -= CloseLoadScreen;
            _loadScreenService.StartCoroutine(AwaitCloseLoadScreen());
        }
    }
}