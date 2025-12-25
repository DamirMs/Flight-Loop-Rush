using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Current.ChickenSkies;
using Gameplay.Current.ChickenSkies.Math;
using Gameplay.Current.ChickenSkies.Rockets;
using Gameplay.General.Game;
using Gameplay.General.Other;
using PT.Tools.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Current
{
    public class GameplayController : LevelGameplayController
    {
        [Inject] private PlaneController _planeController;
        [Inject] private VibroManager _vibroManager;
        [Inject] private SoundManager _soundManager;
        [Inject] private RocketsManager _rocketManager;
        [Inject] private GameMathWindow _mathReplay;
        
        protected override void SignUp()
        {
            base.SignUp();

            _planeController.OnCollisionWithRocket += PlaneCollisionWithRocket;
            _mathReplay.OnSuccess += OnMathSuccess;
            _mathReplay.OnFail += OnMathFail;
        }
        
        protected override void SignOut()
        {
            base.SignOut();
            
            _planeController.OnCollisionWithRocket -= PlaneCollisionWithRocket;
            _mathReplay.OnSuccess -= OnMathSuccess;
            _mathReplay.OnFail -= OnMathFail;
        }
        
        protected override UniTask OnGameTurn(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }

        private void PlaneCollisionWithRocket()
        {
            _vibroManager.Vibrate();
            _soundManager.PlaySound(SoundManager.SoundEventEnum.FinishReached);

            Time.timeScale = 0;
            
            _windowsManager.Open(WindowTypeEnum.GameMathReplay).Forget();
        }
        
        private void OnMathFail()
        {
            GameOver().Forget();
        }
        
        private void OnMathSuccess()
        {
            _rocketManager.ClearRockets(); // just despawn
        }
    }
}