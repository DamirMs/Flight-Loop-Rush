using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.General.Levels;
using Gameplay.General.Score;
using PT.Tools.EventListener;
using Zenject;

namespace Gameplay.General.Game
{
    public class LevelGameplayController : BaseGameplayController
    {
        [Inject] protected LevelsController _levelsController;
        [Inject] protected ScoreManager _scoreManager;
        
        protected override async void OnGameStarted()
        {
            base.OnGameStarted();
        
            // _levelsController.OpenFirstLevel();
        }

        protected override void SignUp()
        {
            _levelsController.OnLevelComplete += OnLevelComplete;
            _levelsController.OnLevelRestart += OnLevelRestart;
            _levelsController.OnLevelFailed += OnLevelFailed;
        }
        
        protected override void SignOut()
        {
            _levelsController.OnLevelComplete -= OnLevelComplete;
            _levelsController.OnLevelRestart -= OnLevelRestart;
            _levelsController.OnLevelFailed -= OnLevelFailed;
        }

        protected virtual void OnLevelComplete()
        {
            _levelsController.OpenNextLevel();
            
            _scoreManager.UpdateScore(_gameInfoConfig.LevelCompletedScore);
            
            GlobalEventBus.On(GlobalEventEnum.LevelStarted);
        }
        protected virtual void OnLevelRestart()
        {
            _levelsController.OpenLastLevel();
        
            GlobalEventBus.On(GlobalEventEnum.LevelStarted);
        }
        protected virtual void OnLevelFailed()
        {
            GameOver();
        }
        
        protected override UniTask OnGameTurn(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }

    }
}