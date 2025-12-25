using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Current.ChickenSkies.Rockets
{
    public class Rocket : MonoBehaviour
    {
        [SerializeField] private LayerMask obstacleCollisionLayer;
        [SerializeField] private ParticleSystem DestroyEffect;
        
        private Transform _target;
        private float _speed;
        private float _rotationValue;
        private float _followingDuration;
        private Action<Rocket> _onDestroyed;
        private Action<Rocket> _onExplodedToDifferentObstacle;
        
        private float _destroyDelay = 2f;
        private bool _destroyTimerStarted;

        private float _timer;

        private CancellationTokenSource _cts;
        
        public void Init(Transform target, float speed, float rotationValue, float followingDuration, Action<Rocket> onDestroyed, Action<Rocket> onExplodedToDifferentObstacle)
        {
            _target = target;
            _speed = speed;
            _rotationValue = rotationValue;
            _followingDuration = followingDuration;
            _onDestroyed = onDestroyed;
            _onExplodedToDifferentObstacle = onExplodedToDifferentObstacle;
            
            _timer = 0;
            _destroyTimerStarted = false;
        }
        
        private void OnDisable()
        {
            _target = null;

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private void Update()
        {
            if (_target != null && _timer < _followingDuration)
            {
                var direction = (_target.position - transform.position).normalized;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                var targetRotation = Quaternion.Euler(0, 0, angle);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationValue * Time.deltaTime);
            }

            transform.position = (Vector2)transform.position + (Vector2)transform.up * _speed * Time.deltaTime;
            _timer += Time.deltaTime;

            if (_timer >= _followingDuration && !_destroyTimerStarted)
            {
                StartDestroyTimer();
            }
        }

        private async void StartDestroyTimer()
        {
            _destroyTimerStarted = true;

            _cts = new();
            
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_destroyDelay), cancellationToken: _cts.Token);
                
                var obj = Instantiate(DestroyEffect).gameObject;
                obj.transform.position = transform.position;
                
                _onDestroyed?.Invoke(this);
            }
            catch (OperationCanceledException) { }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & obstacleCollisionLayer.value) != 0)
            {
                //destroy cause collided with other obstacle / rocket

                var obj = Instantiate(DestroyEffect).gameObject;
                obj.transform.position = transform.position;
                
                _onExplodedToDifferentObstacle?.Invoke(this);
            }
        }
    }
}