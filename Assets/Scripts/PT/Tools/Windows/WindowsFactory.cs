using System;
using System.Collections.Generic;
using PT.Logic.Dependency;
using UnityEngine;
using Zenject;

namespace PT.Tools.Windows
{
    [RequireComponent(typeof(WindowsManager))]
    public class WindowsFactory : MonoBehaviour
    {
        [SerializeField] private Transform parent;

        [Inject] private IFactoryZenject _factoryZenject;

        private readonly Dictionary<WindowTypeEnum, IWindow> _windows = new();

        private void Awake()
        {
            foreach (var window in parent.GetComponentsInChildren<IWindow>())
            {
                _windows[window.Type] = window;
                
                ((MonoBehaviour)window).SetActive(false);
            }
        }
        
        public IWindow GetWindow(WindowTypeEnum type)
        {
            if (!_windows.TryGetValue(type, out var window))
            {
                DebugManager.Log(DebugCategory.UI, $"Window {type} not found in cache", LogType.Error);
                return null;
            }
            
            ((MonoBehaviour)window).transform.SetAsLastSibling();

            return window;   
        }
    }
}