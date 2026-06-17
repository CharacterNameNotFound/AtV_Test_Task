using GameLoop.Entities;
using Utils;

namespace GameLoop.Services
{
    public class EntityResetService : IEntityResetService
    {

        
        public void Reset(BulletComponent bulletComponent)
        {
            bulletComponent.gameObject.SetActive(false);
            bulletComponent.IsCollided = false;
            bulletComponent.Collision = null;
            
            bulletComponent.TrailRenderer.Clear();
        }

        public void Reset(PlayerComponent playerComponent)
        {
            playerComponent.gameObject.SetActive(false);
        }

        public void Reset(StickmanComponent stickmanComponent)
        {
            stickmanComponent.gameObject.SetActive(false);
            stickmanComponent.HpBarRenderer.gameObject.SetActive(false);
            
            stickmanComponent.Animator.SetBool(EnemyAnimationsUtils.EnemyFound, stickmanComponent);
            
            stickmanComponent.IsHpUpdated = false;
            stickmanComponent.IsEnraged = false;
            stickmanComponent.IsPlayerCollided = false;
            stickmanComponent.IsPlayerCollided = false;
            
        }
    }
}