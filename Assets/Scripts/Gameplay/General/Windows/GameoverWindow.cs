using Cysharp.Threading.Tasks;
using PT.Logic.PersistentScene;
using PT.Tools.EventListener;
using PT.Tools.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.General.Windows
{
    public class GameoverWindow : WindowBase
    {
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button menuButton;

        [Inject(Id = "Game")] private WindowsManager _windowsManager;
        [Inject] private SceneLoadManager _sceneLoadManager; 

        private void Awake()
        {
            if (menuButton) menuButton.onClick.AddListener(OnMenuPressed);
            if (playAgainButton) playAgainButton.onClick.AddListener(OnReplayPressed);
        }
        
        private void OnMenuPressed()
        {
            _windowsManager.CloseAllFrom(WindowTypeEnum.GameOver).Forget();
            _sceneLoadManager.LoadScene(SceneNameEnum.Menu, SceneNameEnum.Game).Forget();
            
            GlobalEventBus.On(GlobalEventEnum.GameLeaveToMenu);
        }

        private void OnReplayPressed()
        {
            _windowsManager.CloseAllFrom(WindowTypeEnum.GameOver).Forget();
            _sceneLoadManager.LoadScene(SceneNameEnum.Game, SceneNameEnum.Game).Forget();
        }
    }
}