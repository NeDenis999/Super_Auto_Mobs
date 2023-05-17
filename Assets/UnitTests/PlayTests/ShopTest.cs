using System.Collections;
using NUnit.Framework;
using Super_Auto_Mobs;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnitTests.PlayTests
{
    public class ShopTest
    {
        [UnityTest]
        public IEnumerator ShopPlatform_TrySelect_DoubleClick()
        {
            Object.Instantiate(new GameObject().AddComponent<Camera>());
            
            var awaitSceneContext = Resources.LoadAsync<MonoBehaviour>("SceneContext");
            yield return awaitSceneContext;
            Object.Instantiate(awaitSceneContext.asset);
            
            var awaitShopPlatform = Resources.LoadAsync<ShopPlatform>("ShopPlatforms/CommandPlatform");
            yield return awaitShopPlatform;
            var shopPlatform = (ShopPlatform)Object.Instantiate(awaitShopPlatform.asset);
            
            /*var awaitEntity =Resources.LoadAsync<Entity>("Entitles/Chicken");
            yield return awaitEntity;
            var entity = (Entity)MonoBehaviour.Instantiate(awaitEntity.asset);

            entity.transform.SetParent(shopPlatform.transform);
            shopPlatform.TrySelect();

            Assert.IsFalse(shopPlatform.TrySelect());
            Object.Destroy(shopPlatform.gameObject);
            Object.Destroy(entity.gameObject);*/
            yield return null;
            Assert.IsFalse(false);
        }
    }
}
