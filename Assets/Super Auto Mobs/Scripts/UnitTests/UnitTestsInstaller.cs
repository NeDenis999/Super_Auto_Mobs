using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class UnitTestsInstaller : MonoInstaller
    {        
        [SerializeField]
        private Game _game;
        
        [SerializeField]
        private Shop _shopService;

        /*[SerializeField]
        private ShopTradeService _shopTradeService;
        
        [SerializeField]
        private SessionProgressService _sessionProgressServiceService;
*/
        [SerializeField]
        private BattleService _battleService;
        
        /*[SerializeField]
        private DialogService _dialogService;

        [SerializeField]
        private ShopUpdaterService _shopUpdaterService;

        [SerializeField]
        private MobFactoryService _mobFactoryService;*/

        public override void InstallBindings()
        {
            BindFromInstance(_game);
            BindFromInstance(_shopService);
           // BindFromInstance(_shopTradeService);
            //BindFromInstance(_sessionProgressServiceService);
            BindFromInstance(_battleService);
            //BindFromInstance(_dialogService);
            //BindFromInstance(_shopUpdaterService);
            //BindFromInstance(_mobFactoryService);
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