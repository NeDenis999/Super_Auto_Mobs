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
        
        public override void InstallBindings()
        {
            BindFromInstance(_languageService);
            BindFromInstance(_assetProviderService);
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