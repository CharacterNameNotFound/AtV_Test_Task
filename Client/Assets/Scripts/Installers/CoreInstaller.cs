using Installers;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CoreInstaller", menuName = "Installers/CoreInstaller")]
public class CoreInstaller : ScriptableObjectInstaller<CoreInstaller>
{
    public override void InstallBindings()
    {
        Container.Install<ServiceInstaller>();
        Container.Install<SystemsInstallerInstaller>();
    }
}