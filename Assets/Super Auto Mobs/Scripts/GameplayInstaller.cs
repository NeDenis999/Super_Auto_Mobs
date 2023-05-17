using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField]
        private ShopPlatformService _shopPlatformService;

        [SerializeField]
        private ShopService _shopService;
        
        [SerializeField]
        private SessionProgressService _sessionProgressServiceService;

        [SerializeField]
        private BattleService _battleService;
        
        [SerializeField]
        private DialogService _dialogService;
        
        public override void InstallBindings()
        {
            BindFromInstance(_shopPlatformService);
            BindFromInstance(_shopService);
            BindFromInstance(_sessionProgressServiceService);
            BindFromInstance(_battleService);
            BindFromInstance(_dialogService);
        }

        private void BindFromInstance<T>(T instance)
        {
            Container
                .Bind<T>()
                .FromInstance(instance)
                .AsSingle()
                .NonLazy();
        }
    }
}