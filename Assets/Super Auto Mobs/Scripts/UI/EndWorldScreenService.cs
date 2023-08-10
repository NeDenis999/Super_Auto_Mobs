using UnityEngine;

namespace Super_Auto_Mobs
{
    public class EndWorldScreenService : MonoBehaviour
    {
        [SerializeField]
        private Screen _endWorldScreen, _blackout;

        [SerializeField]
        private SessionProgressService _sessionProgressService;

        private void OnEnable()
        {
            _sessionProgressService.OnUpdateInEndWorld += Open;
        }

        private void OnDisable()
        {
            _sessionProgressService.OnUpdateInEndWorld -= Open;
        }

        public void Open()
        {
            _blackout.Open();
            _endWorldScreen.Open();
        }
    }
}