using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class MusicSlider : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        private SessionProgressService _sessionProgressService;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService)
        {
            _sessionProgressService = sessionProgressService;
        }

        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(UpdateSound);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(UpdateSound);
        }

        private void Start()
        {
            _slider.maxValue = 1;
            _slider.value = _sessionProgressService.Music;
        }
        
        private void UpdateSound(float value)
        {
            _sessionProgressService.Music = _slider.value;
        }
    }
}