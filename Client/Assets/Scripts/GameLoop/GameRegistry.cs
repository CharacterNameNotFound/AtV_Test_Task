using System.Collections.Generic;
using GameLoop.Entities;

namespace GameLoop
{
    public class GameRegistry
    {
        public PlayerComponent PlayerComponent;
        public List<BulletComponent> Bullets;
        public List<StickmanComponent> Stickmans;
    }
}