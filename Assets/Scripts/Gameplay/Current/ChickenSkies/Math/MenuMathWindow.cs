using Cysharp.Threading.Tasks;
using Gameplay.IOS.CurrencyRelated;
using PT.Logic.PersistentScene;
using PT.Tools.Windows;
using Zenject;

namespace Gameplay.Current.ChickenSkies.Math
{
    public class MenuMathWindow : BaseMathWindow
    {
        [Inject(Id = "Menu")] private WindowsManager _windowsManager;
        [Inject] private SceneLoadManager _sceneLoadManager;
        [Inject] private CurrencyManager _currencyManager;

        protected override void OnCorrect()
        {
            DailyMathState.MarkSolved();

            _currencyManager.Add(CurrencyType.Gold, 30);
            
            _windowsManager.Close(WindowTypeEnum.MenuMathReplay).Forget();
            StartGame();
        }

        protected override void OnWrong()
        {
            _windowsManager.Close(WindowTypeEnum.MenuMathReplay).Forget();
            StartGame();
        }

        private void StartGame()
        {
            _sceneLoadManager.LoadScene(SceneNameEnum.Game, SceneNameEnum.Menu).Forget();
        }
    }
}