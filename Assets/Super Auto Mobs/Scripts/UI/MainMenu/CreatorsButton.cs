using UnityEngine;
using UnityEngine.UI;

namespace Super_Auto_Mobs
{
    public class CreatorsButton : MonoBehaviour
    {
        [SerializeField]
        private Screen _creatorsScreen;

        [SerializeField]
        private Canvas _canvas;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Open);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Open);
        }

        private void Open()
        {
            _creatorsScreen.Open();
            _canvas.gameObject.SetActive(false);
        }
    }
}