using UnityEditor;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class ScreensExtensions : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("Tools/ShowShopScreen")]
        public static void ShowShopScreen()
        {
            var shop = FindObjectOfType<Shop>();
            shop.ShopScreen.SetActive(true);
        }
#endif
    }
}