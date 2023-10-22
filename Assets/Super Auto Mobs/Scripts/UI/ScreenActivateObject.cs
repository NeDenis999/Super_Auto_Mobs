using UnityEngine;
using UnityEngine.UI;

namespace Super_Auto_Mobs
{
    public class ScreenActivateObject : MonoBehaviour
    {
        [SerializeField]
        private GameObject _gameObject;
        
        private Screen _screen;

        private void Awake()
        {
            _screen = GetComponent<Screen>();
            
            _screen.OnBeginOpen += Activate;
            _screen.OnFinalyClosing += Deactivate;
        }
        
        private void OnDestroy()
        {
            _screen.OnBeginOpen -= Activate;
            _screen.OnFinalyClosing -= Deactivate;
        }

        private void Activate()
        {
            _gameObject.SetActive(true);   
        }
        
        private void Deactivate()
        {
            _gameObject.SetActive(false);   
        }
    }
}