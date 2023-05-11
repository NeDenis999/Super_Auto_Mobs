using UnityEngine;

namespace Super_Auto_Mobs
{
    public class ShopMobPlatform : ShopPlatform
    {
        [SerializeField]
        private SpriteRenderer _shadow;

        override protected void Select()
        {
            base.Select();
        }
        
        override protected void Unselect()
        {
            base.Unselect();
        }

        override protected void StartMoveEntity()
        {
            _shadow.sprite = _entity.SpriteRenderer.sprite;
            _shadow.gameObject.SetActive(true);
        }

        public override void ResetPosition()
        {
            base.ResetPosition();
            _shadow.gameObject.SetActive(false);
        }
    }
}