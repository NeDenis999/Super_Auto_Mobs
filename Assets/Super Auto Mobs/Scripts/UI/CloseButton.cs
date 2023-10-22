using UnityEngine;
using UnityEngine.UI;

namespace Super_Auto_Mobs
{
    public class CloseButton : MonoBehaviour
    {
        [SerializeField]
        private Screen _screen;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(CloseScreen);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(CloseScreen);
        }

        private void CloseScreen()
        {
            _screen.Close();   
        }
    }
}