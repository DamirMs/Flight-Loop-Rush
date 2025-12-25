using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Current.Configs;
using Gameplay.General.Configs;
using PT.Tools.EventListener;
using UnityEngine;
using Zenject;

namespace Gameplay.General.Levels
{
    public class LevelsController : MonoBehaviourEventListener
    {
        [SerializeField] private Transform levelsParent;

        public event Action OnLevelGoalReached;
        public event Action OnLevelComplete;
        public event Action OnLevelRestart;
        public event Action OnLevelFailed;
        
        [Inject] private LevelsInfoConfig _levelsInfoConfig;
        [Inject] private GameInfoConfig _gameInfoConfig;

        private Level _currentLevel;

        private CancellationTokenSource _cts;
        
        private int LastLevelIndex 
        {
            get => PlayerPrefs.GetInt("LastLevelIndex");
            set => PlayerPrefs.SetInt("LastLevelIndex", value);
        }

        public void OpenFirstLevel()
        {
            CancelDelayBeforeOpeningLevel();

            if (!_levelsInfoConfig.SavesLevel)
            {
                LastLevelIndex = 0;
            }

            OpenLevel(LastLevelIndex);
        }

        public void OpenNextLevel()
        {
            // LastLevelIndex = Mathf.Min(LastLevelIndex + 1, _levelsInfoConfig.LevelsInfo.Levels.Length - 1);
            LastLevelIndex = (LastLevelIndex + 1) % _levelsInfoConfig.LevelsInfo.Levels.Length;

            OpenLevel(LastLevelIndex);
        }
        
        public void OpenLastLevel()
        {
            CancelDelayBeforeOpeningLevel();

            OpenLevel(LastLevelIndex);
        }

        private void OpenLevel(int i)
        {
            CancelDelayBeforeOpeningLevel();

            if (_currentLevel != null) Destroy(_currentLevel.gameObject);
            
            _currentLevel = Instantiate(_levelsInfoConfig.LevelsInfo.Levels[i], levelsParent);

            _currentLevel.Init(LevelComplete, LevelRestart, LevelFailed);
        }

        private async void LevelComplete()
        {
            CancelDelayBeforeOpeningLevel();
            
            _cts = new();

            try
            {
                DebugManager.Log(DebugCategory.Gameplay, $"Level Complete");

                OnLevelGoalReached?.Invoke();
                
                await UniTask.WaitForSeconds(_gameInfoConfig.ConditionMetDelay, cancellationToken: _cts.Token);

                OnLevelComplete?.Invoke();
            }
            catch (Exception e){}
        }
        private void LevelRestart()
        {
            DebugManager.Log(DebugCategory.Gameplay, $"Level Restart");

            OnLevelRestart?.Invoke();
        }
        private void LevelFailed()
        {
            DebugManager.Log(DebugCategory.Gameplay, $"Level Failed");
            
            OnLevelFailed?.Invoke();
        }
        
        private void CancelDelayBeforeOpeningLevel()
        {
            _cts?.Cancel(); _cts?.Dispose(); _cts = null;
        }
    }
}