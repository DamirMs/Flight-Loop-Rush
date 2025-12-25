using Gameplay.General.Levels;
using Gameplay.General.Other;
using UnityEngine;
using Zenject;

namespace Gameplay.General.Sound
{
    public class GoalReachedSoundPlayer : MonoBehaviour
    {
        [Inject] private LevelsController _levelsController;
        [Inject] private SoundManager _soundManager;

        private void OnEnable()
        {
            _levelsController.OnLevelGoalReached += PlaySound;
        }

        private void OnDisable()
        {
            _levelsController.OnLevelGoalReached -= PlaySound;
        }

        private void PlaySound()
        {
            _soundManager.PlaySound(SoundManager.SoundEventEnum.FinishReached);
        }
    }
}