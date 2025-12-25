using Cysharp.Threading.Tasks;
using PT.Logic.PersistentScene;
using PT.Tools.Windows;
using Zenject;

namespace Gameplay.Current
{
    public class MenuMathWindow : BaseMathWindow
    {
        [Inject(Id = "Menu")] private WindowsManager _windowsManager;
        [Inject] private SceneLoadManager _sceneLoadManager;

        protected override void OnCorrect()
        {
            DailyMathState.MarkSolved();

            _windowsManager.Close(WindowTypeEnum.MathReplay).Forget();
            StartGame();
        }

        protected override void OnWrong()
        {
            _windowsManager.Close(WindowTypeEnum.MathReplay).Forget();
            StartGame();
        }

        private void StartGame()
        {
            _sceneLoadManager.LoadScene(SceneNameEnum.Game, SceneNameEnum.Menu).Forget();
        }
    }
}