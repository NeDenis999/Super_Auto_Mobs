using System;
using UnityEngine;
using UnityEngine.UI;

namespace Super_Auto_Mobs
{
    public class WarningScreen : MonoBehaviour
    {
        [SerializeField]
        private Screen _screen;

        [SerializeField]
        private StartScreenService _startScreenService;

        [SerializeField]
        private Button _button;

        private World _world;

        private void OnEnable()
        {
            _button.onClick.AddListener(OpenWorld);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OpenWorld);
        }

        public void Open(World world)
        {
            _world = world;
            _screen.Open();
        }

        private void OpenWorld()
        {
            _startScreenService.OpenWorld(_world);
        }
    }
}