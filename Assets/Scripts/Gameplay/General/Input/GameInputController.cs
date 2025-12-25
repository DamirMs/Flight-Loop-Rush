using System;
using Gameplay.Current.Configs;
using PT.Logic.ProjectContext;
using PT.Tools.EventListener;
using UnityEngine;
using Zenject;

namespace Gameplay.General.Input
{
    public class GameInputController : MonoBehaviourEventListener
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private BoxCollider2D inputZone;
        
        public event Action<Vector2> OnClick;
        public event Action<Vector2> OnDrag;
        public event Action<Vector2> OnRelease;
        
        private bool _listensInput;

        [Inject] private InputManager _inputManager;
        [Inject] private GameInfoConfig _gameInfoConfig;
        
        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted},
                { GlobalEventEnum.GameMenuOpened, OnGameMenuOpened},
                { GlobalEventEnum.GameMenuClosed, OnGameMenuClosed},
                { GlobalEventEnum.GameEnded, OnGameEnded},
            });
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            SignOut();
        }

        public void SetListeningInput()
        {
            if (_gameInfoConfig.ResetsInputAfterRelease)
            {
                _listensInput = true;
                
                DebugManager.Log(DebugCategory.Input, $"GAME listens to input");
            }
        }

        private void OnGameStarted()
        {
            SignUp();
        }
        private void OnGameMenuClosed()
        {
            SignUp();
        }
        private void OnGameMenuOpened()
        {
            SignOut();
        }
        private void OnGameEnded()
        {
            SignOut();
        }

        private void SignUp()
        {
            _inputManager.OnClick += Click;
            _inputManager.OnDrag += Drag;
            _inputManager.OnRelease += Release;
            
            if (!_gameInfoConfig.ResetsInputAfterRelease) _listensInput = true;
        }
        private void SignOut()
        {
            _inputManager.OnClick -= Click;
            _inputManager.OnDrag -= Drag;
            _inputManager.OnRelease -= Release;
            
            if (!_gameInfoConfig.ResetsInputAfterRelease) _listensInput = false;
        }

        private void Click(Vector2 position)
        {
            if (!_listensInput || !InputInZone(position, out Vector2 worldPos)) return;
            DebugManager.Log(DebugCategory.Input, $"GAME click at {worldPos} ({position})");

            OnClick?.Invoke(worldPos);
        }

        private void Drag(Vector2 position)
        {
            if (!_listensInput || !InputInZone(position, out Vector2 worldPos)) return;
            DebugManager.Log(DebugCategory.Input, $"GAME drag at {worldPos} ({position})");

            OnDrag?.Invoke(worldPos);
        }
        
        private void Release(Vector2 position)
        {
            if (!_listensInput || !InputInZone(position, out Vector2 worldPos)) return;
            DebugManager.Log(DebugCategory.Input, $"GAME release at {worldPos} ({position})");

            OnRelease?.Invoke(worldPos);
            
            if (_gameInfoConfig.ResetsInputAfterRelease) _listensInput = false;
        }
        
        private bool InputInZone(Vector2 position, out Vector2 worldPos)
        {
            var screenPos = new Vector3(position.x, position.y, Mathf.Abs(mainCamera.transform.position.z));
            worldPos = mainCamera.ScreenToWorldPoint(screenPos);
            return inputZone.OverlapPoint(worldPos);
        }
    }
}