using System;
using Cysharp.Threading.Tasks;

namespace Gameplay.General.Score
{
    public class ScoreMultiplier
    {
        private float _baseMultiplier = 1f;
        private float _bonusMultiplier = 1f;

        public float Current => _baseMultiplier * _bonusMultiplier;

        public void SetBase(float value) => _baseMultiplier = value;

        public void SetBonus(float value, float duration)
        {
            _bonusMultiplier = value;
            
            Timer(duration); 
        }

        private async void Timer(float seconds)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds));
            _bonusMultiplier = 1f;
        }
    }
}