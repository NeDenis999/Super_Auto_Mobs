using UnityEngine;
using Zenject;
using Slider = UnityEngine.UI.Slider;

namespace Super_Auto_Mobs
{
    public class SoundSlider : MonoBehaviour
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
            _slider.value = _sessionProgressService.Sound;
        }
        
        private void UpdateSound(float value)
        {
            _sessionProgressService.Sound = _slider.value;
        }
    }
}