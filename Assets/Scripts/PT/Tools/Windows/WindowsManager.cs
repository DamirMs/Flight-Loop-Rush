using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PT.Tools.Windows
{
    [RequireComponent(typeof(WindowsFactory))]
    public class WindowsManager : MonoBehaviour
    {
        [SerializeField] private WindowsFactory windowsFactory;
        
        private readonly Stack<IWindow> _stack = new();
        private readonly List<IWindow> _openWindows = new();
        
        public async UniTask<IWindow> Open(WindowTypeEnum type, object data = null)
        {
            var window = windowsFactory.GetWindow(type);

            if (window == null || window.IsOpen || _openWindows.Contains(window)) return window;

            _openWindows.Add(window);
            _stack.Push(window);
            await window.OpenAsync(data);

            return window;
        }

        public async UniTask Close(WindowTypeEnum type)
        {
            var window = windowsFactory.GetWindow(type);

            if (window == null || !window.IsOpen || !_openWindows.Contains(window)) return;
            
            _openWindows.Remove(window);
            RemoveFromStack(window);
            
            await window.CloseAsync();
        }

        public async UniTask CloseTop()
        {
            if (_stack.TryPop(out var top))
            {
                await top.CloseAsync();
                _openWindows.Remove(top);
            }
        }
        
        public async UniTask CloseAllFrom(WindowTypeEnum type)
        {
            while (_stack.Count > 0)
            {
                var top = _stack.Peek();
                await CloseTop();

                if (top.Type == type) break;
            }
        }

        public async UniTask CloseAll()
        {
            while (_stack.Count > 0)
                await CloseTop();
        }

        public IWindow GetLastOpened() => _stack.Count > 0 ? _stack.Peek() : null;
        
        private void RemoveFromStack(IWindow target)
        {
            var temp = new Stack<IWindow>();
            while (_stack.Count > 0)
            {
                var top = _stack.Pop();
                if (top != target)
                    temp.Push(top);
            }
            while (temp.Count > 0)
                _stack.Push(temp.Pop());
        }
    }
}