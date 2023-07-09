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
        private BattleService battleService;
        private GameObject contex;

        [SetUp]
        public void Setup()
        {

        }

        [UnityTest]
        public IEnumerator AddAtkHpPerk_Random_One_Battle()
        {
            yield return AwaitpreParation();
            battleService.SpawnMob(new MobData {MobEnum = MobEnum.Ocelot}, false);
            battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, true);
            yield return battleService.Attack(true);
            yield return battleService.Attack(true);
            Debug.Log("My0 " + battleService.MyCommandMobs[0].CurrentHearts);
            Debug.Log("Enemy1 " + battleService.EnemyCommandMobs[0].CurrentHearts);
            Assert.IsTrue(battleService.MyCommandMobs[0].CurrentHearts == 0);
            Clean();
        }
        
        [UnityTest]
        public IEnumerator AddAtkHpPerk_Random_Two_Battle()
        {
            yield return AwaitpreParation();
            battleService.SpawnMob(new MobData {MobEnum = MobEnum.Ocelot}, false);
            battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, false);
            battleService.SpawnMob(new MobData {MobEnum = MobEnum.Test}, true);
            var startHearts = battleService.MyCommandMobs[1].CurrentHearts;
            yield return battleService.Attack(true);
            yield return battleService.Attack(true);
            Debug.Log("startHearts " + startHearts + " My1 " + battleService.MyCommandMobs[1].CurrentHearts);
            Debug.Log("My0 " + battleService.MyCommandMobs[0].CurrentHearts);
            Debug.Log("Enemy1 " + battleService.EnemyCommandMobs[0].CurrentHearts);
            Assert.Greater(battleService.MyCommandMobs[1].CurrentHearts, startHearts);
            Clean();
        }

        private IEnumerator AwaitpreParation()
        {
            var awaitSceneContext = Resources.LoadAsync<GameObject>("UnitTests/UnitTestsInstaller");
            yield return awaitSceneContext;
            contex = (GameObject)Object.Instantiate(awaitSceneContext.asset);
            var sceneContex = contex.GetComponent<SceneContext>();
            yield return null;
            battleService = sceneContex.Container.Resolve<BattleService>();
            var game = sceneContex.Container.Resolve<Game>();
            game.CurrentGameState = GameState.BattleTransition;
        }

        private void Clean()
        {
            Object.Destroy(contex);
        }
    }
}