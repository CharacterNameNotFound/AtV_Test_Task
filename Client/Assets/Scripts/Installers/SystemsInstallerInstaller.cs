using GameLoop;
using GameLoop.Systems.World;
using Zenject;

namespace Installers
{
    public class SystemsInstallerInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<ILoopedSystem>().To<RoadUpdater>().AsSingle();
        }
        
        
    }
}