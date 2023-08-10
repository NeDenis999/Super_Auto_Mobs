using System.Collections;
using NUnit.Framework;
using Super_Auto_Mobs;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace UnitTests.PlayTests
{
    public class PerkTests
    {
        private const int DelayAnimPerk = 3;
        
        private BattleService battleService;
        private ShopService shopService;
        private Game game;
        private SessionProgressService sessionProgressService;
        private SceneContext sceneContex;
        private GameObject contex;
        
        private CoroutineRunner coroutineRunner;
        private Coroutine _awaitInit;
        private bool _isInit;

        [SetUp]
        public void SetUp()
        {
            Time.timeScale = 10;
            coroutineRunner = Object.Instantiate(new GameObject()).AddComponent<CoroutineRunner>();
            coroutineRunner.gameObject.AddComponent<AudioListener>();
            coroutineRunner.name = "CoroutineRunner";
            coroutineRunner.StartCoroutine(AwaitpreParation());
            _awaitInit = coroutineRunner.StartCoroutine(AwaitInit());
        }
        
        [UnityTest]
        public IEnumerator Bee_Battle()
        {
            yield return _awaitInit;

            //В начале битвы даёт 3 золота
            
            var myMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Bee}, false);
            UpdateState(GameState.Battle);
            yield return null;
            
            Assert.AreNotEqual(sessionProgressService.Gold, 0);
            battleService.RemovePets();
        }

        [UnityTest]
        public IEnumerator Cat_Battle()
        {
            yield return _awaitInit;
            
            //Обморок → Дайте одному случайному другу + 3 Значок атаки и +3 Значок здоровья
            
            var myMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Cat}, false);
            var myMob1 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, false);
            var entityMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, true);
            var startHearts = myMob1.CurrentHearts;
            UpdateState(GameState.Battle);
            
            yield return battleService.Attack(true);
            yield return new WaitForSeconds(DelayAnimPerk);
            
            Debug.Log("startHearts " + startHearts + " My1 " + myMob1.CurrentHearts);
            Debug.Log("My0 " + myMob0.CurrentHearts);
            Debug.Log("Enemy1 " + entityMob0.CurrentHearts);
            Assert.Greater(myMob1.CurrentHearts, startHearts);
            battleService.RemovePets();
        }
        
        [UnityTest]
        public IEnumerator Chokobo_Battle()
        {
            yield return _awaitInit;

            //Упасть в обморок → Дайте двум ближайшим друзьям позади +3 к значку атаки и +3 к значку здоровья
            
            var myMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Chocobo}, false);
            var myMob1 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, false);
            var myMob2 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, false);
            var enemyMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, true);
            UpdateState(GameState.Battle);
            yield return null;

            var myHearts1 = myMob1.CurrentHearts;
            var myHearts2 = myMob2.CurrentHearts;

            yield return battleService.Attack(true);
            yield return new WaitForSeconds(DelayAnimPerk);

            Assert.AreNotEqual(myHearts1 + myHearts2, myMob1.CurrentHearts + myMob2.CurrentHearts);
            battleService.RemovePets();
        }

        [UnityTest]
        public IEnumerator Dog_Shop()
        {
            yield return _awaitInit;
            
            //Продать → Дать двум случайным друзьям + 3 к Значок атаки
            
            UpdateState(GameState.Shop);

            var myMob0 = shopService.SpawnMob(new MobData {MobEnum = MobEnum.Dog});
            var myMob1 = shopService.SpawnMob(new MobData {MobEnum = MobEnum.Test});
            var myMob2 = shopService.SpawnMob(new MobData {MobEnum = MobEnum.Test});

            var startAttacks1 = myMob1.CurrentAttack;
            var startAttacks2 = myMob2.CurrentAttack;

            yield return myMob0.Perk.Activate();
 
            Debug.Log($"currentHearts: {myMob1.CurrentAttack} startHearts: {startAttacks1}");
            Assert.AreNotEqual(myMob1.CurrentAttack + myMob2.CurrentAttack, startAttacks1 + startAttacks2);
            shopService.DestroyPlatformMobs();
        }

        [UnityTest]
        public IEnumerator SnowGollum_Battle()
        {
            yield return _awaitInit;
            
            //Вызванный друг → Дайте ему +3 к Значок атаки до конца битвы
            
            var myMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.SnowGollum}, false);
            var startAttack = battleService.MyCommandMobs[0].CurrentAttack;
            UpdateState(GameState.Battle);

            yield return new WaitForSeconds(2);
            
            Debug.Log("startHearts " + startAttack + " My1 " + myMob0.CurrentAttack);
            Assert.Greater(myMob0.CurrentAttack, startAttack);
            battleService.RemovePets();
        }

        /*[UnityTest]
        public IEnumerator Zombie_Shop()
        {
            yield return _awaitInit;
            
            //Продавать → Дарить магазинным питомцам +3 к здоровью
            
            UpdateState(GameState.Shop);
            var myMob0 = shopService.SpawnMob(new MobData {MobEnum = MobEnum.Zombie});
            var shopMob0 = shopService.SpawnMob(new MobData {MobEnum = MobEnum.Test});
            var shopMob1 = shopService.SpawnMob(new MobData {MobEnum = MobEnum.Test});
            yield return null;
            
            var startHearts0 = shopMob0.CurrentHearts;
            var startHearts1 = shopMob1.CurrentHearts;

            yield return myMob0.Perk.Activate();
            
            Assert.AreNotEqual(shopMob0.CurrentHearts, startHearts0);
            shopService.DestroyPlatformMobs();
        }*/
        
        [UnityTest]
        public IEnumerator Skeleton_Battle()
        {
            yield return _awaitInit;
            
            //Начало битвы → Нанесите 3 Значок поврежденияурон одному случайному врагу
            
            var myMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Skeleton}, false);
            var enemyMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, true);
            UpdateState(GameState.Battle);

            var startHearts = enemyMob0.CurrentHearts;

            yield return new WaitForSeconds(2);
            
            var currentHearts = enemyMob0.CurrentHearts;

            Debug.Log($"currentHearts: {currentHearts} startHearts: {startHearts}");
            Assert.AreNotEqual(currentHearts, startHearts);
            battleService.RemovePets();
        }
                        
        [UnityTest]
        public IEnumerator Duck_Shop()
        {
            yield return _awaitInit;
            
            UpdateState(GameState.Shop);
            
            var myMob0 = shopService.SpawnMob(new MobData {MobEnum = MobEnum.Duck});
            
            yield return myMob0.Perk.Activate();
            
            Assert.True(true);
            shopService.DestroyPlatformMobs();
        }

        [UnityTest]
        public IEnumerator Enderman_Battle()
        {
            yield return _awaitInit;

            //Ранить → Нанести 9 единиц урона одному случайному врагу
            
            var myMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Enderman}, false);
            var enemyMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Dog}, true);
            UpdateState(GameState.Battle);
            yield return null;

            var enemyHearts0 = enemyMob0.CurrentHearts;

            yield return battleService.Attack(true);
            yield return new WaitForSeconds(DelayAnimPerk);

            Assert.AreNotEqual(enemyHearts0, enemyMob0.CurrentHearts);
            battleService.RemovePets();
        }
        
        [UnityTest]
        public IEnumerator MushroomCow_Battle()
        {
            yield return _awaitInit;

            //Ранить → Дайте ближайшему другу сзади +6 к значку атаки и +6 к значку здоровья
            
            var myMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.MushroomCow}, false);
            var myMob1 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, false);
            var enemyMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Dog}, true);
            UpdateState(GameState.Battle);
            yield return null;

            var myHearts1 = myMob1.CurrentHearts;
            
            yield return battleService.Attack(true);
            yield return new WaitForSeconds(DelayAnimPerk);
            
            Assert.AreNotEqual(myMob1.CurrentHearts, myHearts1);
            battleService.RemovePets();
        }
        
        [UnityTest]
        public IEnumerator MilkaCow_Battle()
        {
            yield return _awaitInit;

            //Начало битвы → Дайте 150% иконки атаки ближайшему другу впереди
            
            var myMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, false);
            var myMob1 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.MilkaCow}, false);
            
            var myAttacks0 = myMob0.CurrentAttack;
            UpdateState(GameState.Battle);
            yield return new WaitForSeconds(2);

            Assert.AreNotEqual(myAttacks0, myMob0.CurrentAttack);
            battleService.RemovePets();
        }
        
        [UnityTest]
        public IEnumerator Creeper_Battle()
        {
            yield return _awaitInit;

            //Прежде чем упасть в обморок → Наносит 150% урона от атаки, наносит урон соседним питомцам

            var myMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Creeper}, false);
            var enemyMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, true);
            var enemyMob1 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, true);
            var enemyMob2 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, true);
            var enemyMob3 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, true);
            var enemyMob4 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, true);
            UpdateState(GameState.Battle);
            yield return null;

            var enemyHearts0 = enemyMob0.CurrentHearts;
            var enemyHearts1 = enemyMob1.CurrentHearts;
            var enemyHearts2 = enemyMob2.CurrentHearts;
            var enemyHearts3 = enemyMob3.CurrentHearts;
            var enemyHearts4 = enemyMob4.CurrentHearts;

            yield return battleService.Attack(true);
            yield return null;

            Assert.AreNotEqual(enemyHearts0 + enemyHearts1 + enemyHearts2 + enemyHearts3 + enemyHearts4,
                enemyMob0.CurrentHearts + enemyMob1.CurrentHearts + enemyMob2.CurrentHearts + enemyMob3.CurrentHearts + enemyMob4.CurrentHearts);
            battleService.RemovePets();
        }

        [UnityTest]
        public IEnumerator Squid_Battle()
        {
            yield return _awaitInit;

            //Падает в обморок активирует способность друга сзади

            var myMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Squid}, false);
            var myMob1 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Duck}, false);
            var enemyMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, true);
            UpdateState(GameState.Battle);
            
            yield return null;
            yield return battleService.Attack(true);
            yield return new WaitForSeconds(2);
            
            Assert.AreNotEqual(sessionProgressService.Gold, 0);
            battleService.RemovePets();
        }
                
        [UnityTest]
        public IEnumerator Villager_Battle()
        {
            yield return _awaitInit;
            
            //Обморок → Вызовите одного 1/1 голема

            var myMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Villager}, false);
            var enemyMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, true);
            UpdateState(GameState.Battle);
            
            yield return null;
            yield return battleService.Attack(true);
            yield return new WaitForSeconds(2);
            
            Assert.AreNotEqual(battleService.MyCommandMobs.Count, 1);
            battleService.RemovePets();
        }

        [UnityTest]
        public IEnumerator Witcher_Battle()
        {
            yield return _awaitInit;

            //Начало битвы → Дайте ВСЕМ питомцам +24 к значку здоровья

            var myMob0 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Witch}, false);
            var myMob1 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, false);
            var myMob2 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, false);
            var myMob3 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, false);
            var myMob4 = battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, false);
            
            var hearts = myMob0.CurrentHearts + myMob1.CurrentHearts + myMob2.CurrentHearts + myMob3.CurrentHearts + myMob4.CurrentHearts;
            UpdateState(GameState.Battle);
            
            yield return new WaitForSeconds(2);
            
            Assert.AreNotEqual(hearts, myMob0.CurrentHearts + myMob1.CurrentHearts + myMob2.CurrentHearts + myMob3.CurrentHearts + myMob4.CurrentHearts);
            battleService.RemovePets();
        }
        
        private void UpdateState(GameState gameState)
        {
            if (game.CurrentGameState != gameState)
                game.CurrentGameState = gameState;
        }

        private IEnumerator AwaitInit()
        {
            yield return new WaitUntil(() => _isInit);
        }
        
        private IEnumerator AwaitpreParation()
        {
            if (_isInit)
                yield break;
            
            var awaitSceneContext = Resources.LoadAsync<GameObject>("UnitTests/UnitTestsInstaller");
            yield return awaitSceneContext;
            contex = (GameObject)Object.Instantiate(awaitSceneContext.asset);
            contex.name = "Contex";
            sceneContex = contex.GetComponent<SceneContext>();
            yield return null;
            game = sceneContex.Container.Resolve<Game>();
            battleService = sceneContex.Container.Resolve<BattleService>();
            shopService = sceneContex.Container.Resolve<ShopService>();
            sessionProgressService = sceneContex.Container.Resolve<SessionProgressService>();
            _isInit = true;
        }

        private void Clean()
        {
            
        }
        
        [TearDown]
        public void Teardown()
        {
            _isInit = false;
            Time.timeScale = 1;
            Object.Destroy(contex.gameObject);
            Object.Destroy(coroutineRunner.gameObject);
        }
    }
}