using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Tutorial : MonoBehaviour
    {
        private DialogService _dialogService;

        [Inject]
        private void Construct(DialogService dialogService)
        {
            _dialogService = dialogService;
        }

        private void Start()
        {
            _dialogService.Show();
        }
    }
}