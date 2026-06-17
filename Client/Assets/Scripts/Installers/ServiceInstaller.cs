using GameCore;
using GameLoop;
using GameLoop.Entities;
using GameLoop.Services;
using UI;
using Utils;
using VFX.ColorBlink;
using VFX.FlyingText;
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
            Container.Bind<IGameProgressProvider>().To<GameProgressProvider>().AsSingle();
            
            // Utils
            Container.Bind<GameRootTransformProvider>().To<GameRootTransformProvider>().AsSingle();
            Container.Bind<MainUIControllerHolder>().To<MainUIControllerHolder>().AsSingle();
            Container.Bind<IEntityResetService>().To<EntityResetService>().AsSingle();
            
            // VFX
            Container.Bind<ColorBlinkAnimationManager>().To<ColorBlinkAnimationManager>().AsSingle();
            Container.Bind<FlyingTextManager>().To<FlyingTextManager>().AsSingle();
        }

        private void InstallPools()
        {
            Container.Bind<GameObjectPool<StickmanComponent>>().To<GameObjectPool<StickmanComponent>>().AsSingle();
            Container.Bind<GameObjectPool<BulletComponent>>().To<GameObjectPool<BulletComponent>>().AsSingle();
        }
    }
}