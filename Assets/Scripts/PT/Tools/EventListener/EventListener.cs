using System;
using System.Collections.Generic;

namespace PT.Tools.EventListener
{
    public abstract class EventActionMap
    {
        protected Dictionary<GlobalEventEnum, Action> _eventActionDict = new();

        public void AddEventActions(Dictionary<GlobalEventEnum, Action> eventActionDict)
        {
            if (eventActionDict == null) return;

            foreach (var kvp in eventActionDict)
            {
                if (_eventActionDict.ContainsKey(kvp.Key) && _eventActionDict[kvp.Key] != kvp.Value)
                {
                    _eventActionDict[kvp.Key] += kvp.Value;
                }
                else
                {
                    _eventActionDict.Add(kvp.Key, kvp.Value);
                }
            }
        }

        public void Subscribe() => GlobalEventBus.OnEventHandle += OnEvent;
        public void Unsubscribe() => GlobalEventBus.OnEventHandle -= OnEvent;
    
        protected virtual void OnEvent(GlobalEventEnum eventEnum)
        {
            if (_eventActionDict.TryGetValue(eventEnum, out var action)) action?.Invoke();
        }
    }

    public class EventListener : EventActionMap
    {
        public EventListener(Dictionary<GlobalEventEnum, Action> eventActionDict, bool autoSubscribe = false)
        {
            AddEventActions(eventActionDict);

            if (autoSubscribe) Subscribe();
        }

        ~EventListener()
        {
            Unsubscribe();
        }
    }
}