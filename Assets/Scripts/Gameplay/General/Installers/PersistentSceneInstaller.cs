using PT.Logic.PersistentScene;
using PT.Tools.Windows;
using Zenject;

namespace Gameplay.General.Installers
{
    public class PersistentSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<WindowsManager>().WithId("Persistent").FromComponentInHierarchy().AsSingle();
            
            Container.Bind<LoadingWindowManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}