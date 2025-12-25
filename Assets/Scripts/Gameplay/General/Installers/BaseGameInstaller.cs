using Gameplay.Current.Configs;
using Gameplay.General.Configs;
using Gameplay.General.Input;
using Gameplay.General.Levels;
using Gameplay.General.Other;
using Gameplay.General.Score;
using Gameplay.General.Windows;
using PT.Logic.ProjectContext;
using PT.Tools.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.General.Installers
{
    public class BaseGameInstaller : MonoInstaller
    {
        [SerializeField] private LevelsInfoConfig levelsInfoConfig; 
        
        public override void InstallBindings()
        {
            Container.Bind<WindowsManager>().WithId("Game").FromComponentInHierarchy().AsSingle();
            
            Container.Bind<GameWindowsController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ScoreManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<InputManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameInputController>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<SoundManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<VibroManager>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<LevelsController>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<LevelsInfoConfig>().FromInstance(levelsInfoConfig).AsSingle();
            
            Container.Bind<ScoreMultiplier>().AsSingle();
        }
    }
}