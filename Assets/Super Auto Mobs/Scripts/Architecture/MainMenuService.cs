using System.Collections.Generic;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class MainMenuService : MonoBehaviour
    {
        [SerializeField]
        private GameObject _menu;

        [SerializeField]
        private Camera _myCamera;
        
        [SerializeField]
        private Camera _menuCamera;

        [SerializeField]
        private List<GameObject> _offObjects;

        public void Open()
        {
            _menu.SetActive(true);

            foreach (var offObject in _offObjects)
            {
                offObject.SetActive(false);
            }
            
            _myCamera.gameObject.SetActive(false);
            _menuCamera.gameObject.SetActive(true);
        }
        
        public void Close()
        {
            if (!_menu.activeSelf)
                return;

            _menu.SetActive(false);
            
            foreach (var offObject in _offObjects)
            {
                offObject.SetActive(true);
            }
            
            _myCamera.gameObject.SetActive(true);
            _menuCamera.gameObject.SetActive(false);
        }
    }
}