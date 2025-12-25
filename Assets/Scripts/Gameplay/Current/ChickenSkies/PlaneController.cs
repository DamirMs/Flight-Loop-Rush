using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Current.Configs;
using PT.Tools.EventListener;
using UnityEngine;
using Zenject;

namespace Gameplay.Current.ChickenSkies
{
    public class PlaneController : MonoBehaviourEventListener
    {
        [SerializeField] private Transform planeTransform;
        [SerializeField] private LayerMask rocketCollisionLayer;
        [SerializeField] private Transform initialPosition;
        [SerializeField] private Joystick joystick;
        [SerializeField] private Transform planeVisual;

        [Header("Boost")]
        [SerializeField] private float boostMultiplier = 2f;
        [SerializeField] private float boostDuration = 0.5f;
        [SerializeField] private float boostCooldown = 3f;
        
        public bool IsBoostAvailable => !_boostActive && !_boostOnCooldown;
        public float BoostCooldown => boostCooldown;
        public float BoostDuration => boostDuration;

        [Inject] private GameInfoConfig _gameInfoConfig;

        public event Action OnCollisionWithRocket;
        public Transform PlaneTransform => planeTransform;

        private Vector2 _currentMoveDirection;
        private float _currentSpeedMultiplier = 1f;

        private bool _boostActive;
        private bool _boostOnCooldown;

        private CancellationTokenSource _boostCts;

        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.GameEnded, OnGameEnded },
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _boostCts?.Cancel();
            _boostCts?.Dispose();
            _boostCts = null;
        }

        private void OnGameStarted()
        {
            transform.position = initialPosition.position;
            _currentMoveDirection = Vector2.up;
        }

        private void OnGameEnded()
        {
            transform.position = initialPosition.position;
            joystick.OnPointerUp(null);

            _boostCts?.Cancel();
            _currentSpeedMultiplier = 1f;
            _boostActive = false;
            _boostOnCooldown = false;
        }

        private void Update()
        {
            var direction = new Vector2(joystick.Horizontal, joystick.Vertical);
            if (direction != Vector2.zero)
                _currentMoveDirection = direction;

            planeTransform.position +=
                (Vector3)(_currentMoveDirection.normalized *
                          _gameInfoConfig.PlaneSpeed *
                          _currentSpeedMultiplier *
                          Time.deltaTime);
            
            RotateVisual(_currentMoveDirection);
        }
        private void RotateVisual(Vector2 dir)
        {
            if (dir.sqrMagnitude < 0.0001f) return;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            planeVisual.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public void TryBoost()
        {
            if (_boostActive || _boostOnCooldown) return;

            _boostCts?.Cancel();
            _boostCts = new();

            Boost(_boostCts.Token).Forget();
        }

        private async UniTaskVoid Boost(CancellationToken token)
        {
            _boostActive = true;
            _boostOnCooldown = true;

            _currentSpeedMultiplier = boostMultiplier;

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(boostDuration), cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                ResetBoostState();
                return;
            }

            _currentSpeedMultiplier = 1f;
            _boostActive = false;

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(boostCooldown), cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                ResetBoostState();
                return;
            }

            _boostOnCooldown = false;
        }

        private void ResetBoostState()
        {
            _currentSpeedMultiplier = 1f;
            _boostActive = false;
            _boostOnCooldown = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & rocketCollisionLayer.value) != 0)
            {
                OnCollisionWithRocket?.Invoke();
            }
        }
    }
}
