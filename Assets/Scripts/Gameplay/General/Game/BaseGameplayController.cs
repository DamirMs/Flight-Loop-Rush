using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Current.Configs;
using Gameplay.General.Windows;
using PT.Tools.EventListener;
using PT.Tools.Windows;
using Zenject;

namespace Gameplay.General.Game
{
    public abstract class BaseGameplayController : MonoBehaviourEventListener
    {
        [Inject] protected GameInfoConfig _gameInfoConfig;
        [Inject(Id = "Game")] protected WindowsManager _windowsManager;
        
        private CancellationTokenSource _turnCts;
        private CancellationTokenSource _overCts;

        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted},
                { GlobalEventEnum.GameEnded, OnGameEnded},
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            SignOut();
        }

        protected virtual async void OnGameStarted()
        {
            SignUp();
            
            await UniTask.DelayFrame(1);
        }

        protected virtual void OnGameEnded()
        {
            SignOut();

            CancelTurn();
        }

        protected async UniTaskVoid GameTurn()
        {
            CancelTurn();

            _turnCts = new();
            try
            {
                GlobalEventBus.On(GlobalEventEnum.GameTurn);
                await OnGameTurn(_turnCts.Token);
            }
            catch (OperationCanceledException) { }
        }

        protected async UniTaskVoid GameOver()
        {
            CancelGameOver();

            _overCts = new();
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_gameInfoConfig.GameOverDelay), cancellationToken: _overCts.Token);
                
                _windowsManager.CloseAll().Forget();
                _windowsManager.Open(WindowTypeEnum.GameOver).Forget();
                
                GlobalEventBus.On(GlobalEventEnum.GameEnded);
            }
            catch (OperationCanceledException) { }
        }

        protected void CancelTurn()
        {
            _turnCts?.Cancel(); _turnCts?.Dispose(); _turnCts = null;
        }
        protected void CancelGameOver()
        {
            _overCts?.Cancel(); _overCts?.Dispose(); _overCts = null;
        }

        protected abstract void SignUp();
        protected abstract void SignOut();
        
        protected abstract UniTask OnGameTurn(CancellationToken token);
    }
}