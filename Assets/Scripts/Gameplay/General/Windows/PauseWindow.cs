using Cysharp.Threading.Tasks;
using PT.Logic.PersistentScene;
using PT.Tools.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.General.Windows
{
    public class PauseWindow : WindowBase
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button replayButton;
        [SerializeField] private Button menuButton;

        [Inject] private SceneLoadManager _sceneLoadManager; 
        [Inject(Id = "Game")] private WindowsManager _windowsManager; 
        
        private void Awake()
        {
            resumeButton.onClick.AddListener(OnResume);
            replayButton.onClick.AddListener(OnReplay);            
            menuButton.onClick.AddListener(OnMenu);            
        }

        private void OnResume()
        {
            _windowsManager.CloseAllFrom(WindowTypeEnum.Pause).Forget();
        }
        private void OnReplay()
        {
            _windowsManager.CloseAllFrom(WindowTypeEnum.Pause).Forget();
            
            _sceneLoadManager.LoadScene(SceneNameEnum.Menu, SceneNameEnum.Game).Forget();
        }
        private void OnMenu()
        {
            _windowsManager.CloseAllFrom(WindowTypeEnum.Pause).Forget();
            
            _sceneLoadManager.LoadScene(SceneNameEnum.Menu, SceneNameEnum.Game).Forget();
            
            _windowsManager.Open(WindowTypeEnum.Menu).Forget();
        }
    }
}