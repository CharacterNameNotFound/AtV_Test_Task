using GameCore;
using GameLoop;
using GameLoop.Entities;
using Utils;
using Zenject;

namespace Installers
{
    public class ServiceInstaller : Installer
    {
        public override void InstallBindings()
        {
            InstallServices();
            InstallPools();
        }

        private void InstallServices()
        {
            Container.Bind<IGameRoot>().To<GameRoot>().AsSingle();
            Container.Bind<IGameLooper>().To<GameLooper>().AsSingle();
            Container.Bind<IGameEndDecisionMaker>().To<GameEndDecisionMaker>().AsSingle();
            Container.Bind<GameRegistry>().To<GameRegistry>().AsSingle();
        }

        private void InstallPools()
        {
            Container.Bind<GameObjectPool<StickmanComponent>>().To<GameObjectPool<StickmanComponent>>().AsSingle();
            Container.Bind<GameObjectPool<BulletComponent>>().To<GameObjectPool<BulletComponent>>().AsSingle();
        }
    }
}