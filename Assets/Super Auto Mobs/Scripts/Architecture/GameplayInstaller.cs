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
        private SessionProgressService _sessionProgressService;

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
        private StartScreenService startScreenService;
        
        [SerializeField]
        private TitlesService _titlesService;
        
        [SerializeField]
        private LanguageService _languageService;

        [SerializeField]
        private AssetProviderService _assetProviderService;

        [SerializeField]
        private LoaderLevelService _loaderLevelService;

        [SerializeField]
        private SoundsService _soundsService;

        [SerializeField]
        private LoadScreenService _loadScreenService;

        [SerializeField]
        private CoroutineRunner _coroutineRunner;
        
        public override void InstallBindings()
        {
            BindFromInstance(_game);
            BindFromInstance(_shopService);
            BindFromInstance(_shopTradeService);
            BindFromInstance(_sessionProgressService);
            BindFromInstance(_battleService);
            BindFromInstance(_dialogService);
            BindFromInstance(_shopUpdaterService);
            BindFromInstance(_mobFactoryService);
            BindFromInstance(_sparkService);
            BindFromInstance(startScreenService);
            BindFromInstance(_titlesService);
            BindFromInstance(_languageService);
            BindFromInstance(_assetProviderService);
            BindFromInstance(_loaderLevelService);
            BindFromInstance(_soundsService);
            BindFromInstance(_loadScreenService);
            BindFromInstance(_coroutineRunner);
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