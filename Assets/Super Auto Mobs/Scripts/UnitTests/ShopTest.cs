using System;
using System.Collections.Generic;

namespace Super_Auto_Mobs
{
    public class ShopTest : Shop
    {
        public override event Action OnSelectCommandPlatform;
        public override event Action OnUnselectCommandPlatform;
        public override event Action<PlatformServiceState> OnUpdateState;

        public override List<ShopCommandMobPlatform> CommandPlatforms { get; }
        public override ShopPlatform ShopPlatformSelected { get; }
        
        public override void Open()
        {
            gameObject.SetActive(true);
        }

        public override void Close()
        {
            gameObject.SetActive(false);
        }

        public override void DestroySelectEntity()
        {
            
        }
    }
}