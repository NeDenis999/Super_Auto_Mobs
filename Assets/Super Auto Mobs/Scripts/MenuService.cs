using System;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class MenuService : MonoBehaviour
    {
        [SerializeField]
        private GameObject _menu, _upPanel;

        public GameObject Menu => _menu;

        public void Open()
        {
            _upPanel.SetActive(true);
        }
        
        public void Close()
        {
            _upPanel.SetActive(false);
        }
    }
}