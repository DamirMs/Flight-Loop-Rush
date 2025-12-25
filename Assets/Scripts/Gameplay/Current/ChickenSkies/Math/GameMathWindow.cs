using System;
using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using Zenject;

namespace Gameplay.Current
{
    public class GameMathWindow : BaseMathWindow
    {
        [Inject(Id = "Game")] private WindowsManager _windowsManager;

        public event Action OnSuccess;
        public event Action OnFail;

        protected override void OnCorrect()
        {
            _currentTime = _currentTime <= 0 ? startTime : _currentTime;
            _currentTime *= timeMultiplier;

            _windowsManager.Close(WindowTypeEnum.MathReplay).Forget();
            OnSuccess?.Invoke();
        }

        protected override void OnWrong()
        {
            _windowsManager.Close(WindowTypeEnum.MathReplay).Forget();
            OnFail?.Invoke();
        }
    }
}