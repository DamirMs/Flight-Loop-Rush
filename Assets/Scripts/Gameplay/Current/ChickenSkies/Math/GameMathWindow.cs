using System;
using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Current.ChickenSkies.Math
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

            _windowsManager.Close(WindowTypeEnum.GameMathReplay).Forget();
            OnSuccess?.Invoke();
            Time.timeScale = 1;
        }

        protected override void OnWrong()
        {
            _windowsManager.Close(WindowTypeEnum.GameMathReplay).Forget();
            OnFail?.Invoke();
            Time.timeScale = 1;
        }
    }
}