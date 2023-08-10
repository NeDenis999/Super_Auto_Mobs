using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class InShopButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        
        private Game _game;

        [Inject]
        private void Construct(Game game)
        {
            _game = game;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OpenShop);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OpenShop);
        }

        private void OpenShop()
        {
            _game.CurrentGameState = GameState.Shop;
        }
    }
}