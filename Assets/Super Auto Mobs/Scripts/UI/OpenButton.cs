using UnityEngine;
using UnityEngine.UI;

namespace Super_Auto_Mobs
{
    public class OpenButton : MonoBehaviour
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
            _button.onClick.AddListener(OpenScreen);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OpenScreen);
        }

        private void OpenScreen()
        {
            _screen.Open();   
        }
    }
}