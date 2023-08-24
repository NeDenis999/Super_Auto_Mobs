using System;

namespace Super_Auto_Mobs
{
    [Serializable]
    public struct Discount
    {
        public PurchaseEnum ProductType;
        public int Amount;
    }
}