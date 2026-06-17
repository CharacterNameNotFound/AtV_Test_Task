using GameLoop.Entities;

namespace GameLoop.Services
{
    public interface IEntityResetService
    {
        public void Reset(BulletComponent bulletComponent);
        public void Reset(PlayerComponent playerComponent);
        public void Reset(StickmanComponent stickmanComponent);

    }
}