using System;
using System.Collections.Generic;
using UnityEngine;

namespace PT.Tools.EventListener
{
    public abstract class MonoBehaviourEventListener  : MonoBehaviour
    {
        private readonly EventActionMap _eventActionMap = new global::PT.Tools.EventListener.EventListener(new Dictionary<GlobalEventEnum, Action>());

        protected void AddEventActions(Dictionary<GlobalEventEnum, Action> eventActionDict) => _eventActionMap.AddEventActions(eventActionDict);

        protected virtual void OnEnable() => _eventActionMap.Subscribe();
        protected virtual void OnDisable() => _eventActionMap.Unsubscribe();
    }
}