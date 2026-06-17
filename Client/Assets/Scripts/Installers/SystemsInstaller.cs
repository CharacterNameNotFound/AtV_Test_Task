using GameLoop;
using GameLoop.Systems.Enemy;
using GameLoop.Systems.Player;
using GameLoop.Systems.World;
using Zenject;

namespace Installers
{
    public class SystemsInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerMover>().To<PlayerMover>().AsSingle();
            
            
            Container.Bind<ILoopedSystem>().To<RoadUpdater>().AsCached();
            Container.Bind<ILoopedSystem>().To<EnemySpawner>().AsCached();
            Container.Bind<ILoopedSystem>().To<EnemyMover>().AsCached();
            
            Container.Bind<ILoopedSystem>().To<PlayerMover>().FromResolve().AsCached();
            Container.Bind<ILoopedSystem>().To<PlayerBulletsMover>().AsCached();
            Container.Bind<ILoopedSystem>().To<PlayerShooter>().AsCached();
            
            Container.Bind<ILoopedSystem>().To<EnemyHpUpdater>().AsCached();
            Container.Bind<ILoopedSystem>().To<PlayerHpUpdater>().AsCached();
        }
        
        
    }
}