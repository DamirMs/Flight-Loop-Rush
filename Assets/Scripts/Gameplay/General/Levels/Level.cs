using System;
using UnityEngine;

namespace Gameplay.General.Levels
{
    public class Level : MonoBehaviour
    {
        private Action _nextAction;
        private Action _replayAction;
        private Action _failedAction;

        public void Init(Action nextAction, Action replayAction, Action failedAction)
        {
            _nextAction = nextAction;
            _replayAction = replayAction;
            _failedAction = failedAction;
        }

        public void LevelComplete()
        {
            _nextAction.Invoke();
        }

        public void LevelRestart()
        {
            _replayAction.Invoke();
        }
        
        public void LevelFailed()
        {
            _failedAction.Invoke();
        }
    }
}