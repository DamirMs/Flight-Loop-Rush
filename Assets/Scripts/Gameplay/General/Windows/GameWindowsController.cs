using System;
using System.Linq;
using PT.Tools.EventListener;
using PT.Tools.Helper;
using UnityEngine;

namespace Gameplay.General.Windows
{
    public class GameWindowsController : MonoBehaviour
    {
        [SerializeField] private SerializableKeyValue<SerializableGlobalEventEnum, SerializableObjects> windows;

        [Serializable]
        class SerializableGlobalEventEnum
        {
            [SerializeField] private GlobalEventEnum[] globalEventEnums;
            
            public  GlobalEventEnum[] Keys => globalEventEnums;
        }
        
        [Serializable]
        class SerializableObjects
        {
            [SerializeField] private GameObject[] objects;

            public void SetActive(bool active) => objects.SetActive(active);
        }
        
        private void Awake()
        {
            foreach (var group in  windows.Dictionary.Values) 
                group.SetActive(false);
        }

        private void OnEnable()
        {
            Open(GlobalEventEnum.GameEnter);
        }

        public void Open(GlobalEventEnum target)
        {
            ShowWindow(target);
            
            GlobalEventBus.On(target);
            
            HideAllExcept(target);
        }

        private void ShowWindow(GlobalEventEnum key)
        {
            foreach (var kvp in windows.Dictionary)
            {
                if (kvp.Key.Keys.Any(x => x == key))
                {
                    kvp.Value.SetActive(true); break;
                }
            }
        }

        private void HideAllExcept(GlobalEventEnum except)
        {
            foreach (var kvp in windows.Dictionary)
            {
                if (kvp.Key.Keys.Any(x => x == except)) continue;
                
                kvp.Value.SetActive(false);
            }
        }
    }
}