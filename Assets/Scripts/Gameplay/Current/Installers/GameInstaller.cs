using Gameplay.Current.ChickenSkies;
using Gameplay.Current.ChickenSkies.Math;
using Gameplay.Current.ChickenSkies.Rockets;
using Gameplay.General.Installers;

namespace Gameplay.Current.Installers
{
    public class GameInstaller : BaseGameInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.Bind<PlaneController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameMathWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RocketsManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}