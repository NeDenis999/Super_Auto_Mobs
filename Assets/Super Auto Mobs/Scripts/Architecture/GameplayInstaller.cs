using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField]
        private Game _game;
        
        [SerializeField]
        private ShopService _shopService;

        [SerializeField]
        private ShopTradeService _shopTradeService;
        
        [SerializeField]
        private SessionProgressService _sessionProgressServiceService;

        [SerializeField]
        private BattleService _battleService;
        
        [SerializeField]
        private DialogService _dialogService;

        [SerializeField]
        private ShopUpdaterService _shopUpdaterService;

        [SerializeField]
        private MobFactoryService _mobFactoryService;

        [SerializeField]
        private SparkService _sparkService;

        [SerializeField]
        private MainMenuService _mainMenuService;
        
        [SerializeField]
        private TitlesService _titlesService;
        
        public override void InstallBindings()
        {
            BindFromInstance(_game);
            BindFromInstance(_shopService);
            BindFromInstance(_shopTradeService);
            BindFromInstance(_sessionProgressServiceService);
            BindFromInstance(_battleService);
            BindFromInstance(_dialogService);
            BindFromInstance(_shopUpdaterService);
            BindFromInstance(_mobFactoryService);
            BindFromInstance(_sparkService);
            BindFromInstance(_mainMenuService);
            BindFromInstance(_titlesService);
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