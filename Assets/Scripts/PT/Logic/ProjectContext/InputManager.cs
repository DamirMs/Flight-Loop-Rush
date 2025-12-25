using System;
using PT.Tools.EventListener;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PT.Logic.ProjectContext 
{
    public class InputManager : InputManagerBase
    {
        public event Action<Vector2> OnClick;
        public event Action<Vector2> OnDrag;
        public event Action<Vector2> OnRelease;
        
        private InputActionMap _inGameInput;

        private bool _isPressing;
        private Vector2 _lastPointerPosition;       
        
        public void Init()
        {
            AddEventActions(new ()
            {
                { GlobalEventEnum.GameMenuClosed, () => EnableActionMap(_inGameInput) },
            });
            
        }

        protected override void OnStart()
        {
            _inGameInput = _input.InGameInput;
            _actionMapsList.Add(_inGameInput);

            EnableActionMap(_inGameInput);
        }
        
        protected override void OnSub()
        {
            var input = _input.InGameInput;

            Subscribe(input.Click, ctx =>
            {
                _isPressing = true;
                
                OnClick?.Invoke(GetPointerPosition());
            });

            Subscribe(input.Drag, ctx =>
            {
                if (!_isPressing) return;
                
                OnDrag?.Invoke(GetPointerPosition());
            });

            Subscribe(input.Release, ctx =>
            {
                if (!_isPressing) return;
                _isPressing = false;
                
                OnRelease?.Invoke(_lastPointerPosition);
            });
        }
        
        private Vector2 GetPointerPosition()
        {
            if (Touchscreen.current != null)
                _lastPointerPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            else if (Mouse.current != null)
                _lastPointerPosition = Mouse.current.position.ReadValue();
            return _lastPointerPosition;
        }

        protected override void OnUnSub()
        {
        }
    }
}