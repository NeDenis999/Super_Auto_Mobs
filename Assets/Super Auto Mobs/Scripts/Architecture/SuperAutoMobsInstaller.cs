using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs.Scripts
{
    public class SuperAutoMobsInstaller : MonoInstaller
    {
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

        public override void InstallBindings()
        {
            BindFromInstance(_languageService);
            BindFromInstance(_assetProviderService);
            BindFromInstance(_loaderLevelService);
            BindFromInstance(_soundsService);
            BindFromInstance(_loadScreenService);
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