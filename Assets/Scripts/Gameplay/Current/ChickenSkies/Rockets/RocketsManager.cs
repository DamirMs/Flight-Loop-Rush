using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Current.Configs;
using Gameplay.General.Score;
using PT.Tools.EventListener;
using PT.Tools.ObjectPool;
using UnityEngine;
using Zenject;

namespace Gameplay.Current.ChickenSkies.Rockets
{
    public class RocketsManager : MonoBehaviourEventListener
    {
        [SerializeField] private Rocket rocketPrefab;
        [SerializeField] private Transform rocketsParent;
        
        [Inject] private GameInfoConfig _gameInfoConfig;
        [Inject] private PlaneController _planeController;
        [Inject] private ScoreManager _scoreManager;
        
        private MonoBehPool<Rocket> _rocketPool = new();
        
        private List<Rocket> _currentRockets = new();

        private CancellationTokenSource _spawningCts;
        
        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.GameEnded, OnGameEnded },
            });
            
            _rocketPool.Init(rocketPrefab.gameObject, rocketsParent, 20);
        }
        
        private void OnGameStarted()
        {
            StartSpawningRockets().Forget();
        }
        private void OnGameEnded()
        {
            ClearRockets();
            
            _spawningCts?.Cancel(); 
            _spawningCts?.Dispose();
            _spawningCts = null;
        }

        private async UniTask StartSpawningRockets()
        {
            _spawningCts = new();
    
            var delayMinMax = _gameInfoConfig.RocketsSpawnDelay;
            
            try
            {
                while (!_spawningCts.IsCancellationRequested)
                {
                    var spawnPos = GetRandomSpawnAroundPlane(); 
                    CreateRocket(spawnPos);

                    await UniTask.Delay(TimeSpan.FromSeconds(delayMinMax.GetRandomValue()), cancellationToken: _spawningCts.Token);

                    delayMinMax = new (Mathf.Max(0.1f, delayMinMax.x - _gameInfoConfig.RocketsSpawnDecreaseValue), Mathf.Max(0.1f, delayMinMax.y - _gameInfoConfig.RocketsSpawnDecreaseValue));
                }
            }
            catch (OperationCanceledException) {}
        }
        
        private Vector2 GetRandomSpawnAroundPlane()
        {
            var offset = UnityEngine.Random.insideUnitCircle.normalized * 10f;
            return (Vector2)_planeController.PlaneTransform.position + offset;
        }
        
        private void CreateRocket(Vector2 position)
        {
            var rocket = _rocketPool.Get();

            rocket.transform.position = position;
            rocket.Init(_planeController.PlaneTransform, _gameInfoConfig.RocketsSpeeds.GetRandomValue(), 
                _gameInfoConfig.RocketRotationValue, _gameInfoConfig.FollowingDurations.GetRandomValue(),
                ReturnRocket, RocketExplodedToRocket);
            
            _currentRockets.Add(rocket);
        }

        private void ReturnRocket(Rocket rocket)
        {
            _currentRockets.Remove(rocket);
            
            _rocketPool.Set(rocket);
        }
        
        private void RocketExplodedToRocket(Rocket rocket)
        {
            ReturnRocket(rocket);
            
            _scoreManager.UpdateScore(50);
        }

        public void ClearRockets()
        {
            while (_currentRockets.Count > 0)
            {
                ReturnRocket(_currentRockets[0]);
            }

            _currentRockets.Clear();
        }
    }
}