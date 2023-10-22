namespace Super_Auto_Mobs
{
    public class TagBuffEffect : BuffEffect
    {
        private float _startY;
        
        public override void Activate(Mob mob)
        {
            var spriteRenderer = mob.SpriteRenderer;
            spriteRenderer.flipY = true;
            var localPosition = spriteRenderer.gameObject.transform.localPosition;
            _startY = localPosition.y;
            spriteRenderer.gameObject.transform.localPosition = localPosition.AddY(spriteRenderer.sprite.rect.height / 20f);
        }

        public override void Deactivate(Mob mob)
        {
            var spriteRenderer = mob.SpriteRenderer;
            spriteRenderer.flipY = false;
            var localPosition = spriteRenderer.gameObject.transform.localPosition;
            spriteRenderer.gameObject.transform.localPosition = localPosition.SetY(_startY);
        }
    }
}