using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.General.Score;
using PT.Tools.EventListener;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.General.Other
{
    public class TimerScoreUpdater : MonoBehaviourEventListener
    {
        [SerializeField] private float delay = 1;
        [SerializeField] private int scoreUpdate = 1;

        public IReadOnlyReactiveProperty<float> CurrentTime => _currentTime;
        private readonly ReactiveProperty<float> _currentTime = new();
        
        [Inject] private ScoreManager _scoreManager;
        
        private CancellationTokenSource _cts;
        
        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.GameEnded, OnGameEnded },
            });
        }
        
        private void OnGameStarted()
        {
            _currentTime.Value = 0;
            
            StartTimer().Forget();
        }
        private void OnGameEnded()
        {
            _currentTime.Value = 0;
            
            _cts?.Cancel(); _cts?.Dispose(); _cts = null;
        }

        private async UniTask StartTimer()
        {
            _cts = new();

            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: _cts.Token);
                    
                    if (_cts.IsCancellationRequested) return;

                    _scoreManager.UpdateScore(scoreUpdate);
                }
            }
            catch (Exception ex) {}
        }
    }
}