using Gameplay.IOS.CurrencyRelated;
using PT.Tools.EventListener;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.General.Score
{
    public class ScoreManager : MonoBehaviourEventListener
    {
        public ReactiveProperty<int> TotalScoreReactive { get; private set; } = new();
        public ReactiveProperty<int> BestScoreReactive { get; private set; } = new();

        [Inject] private ScoreMultiplier _multiplier;        
        [Inject] private CurrencyManager _currencyManager;
        
        private int BestScore 
        {
            get => _bestScore;
            set
            {
                _bestScore = value;
                BestScoreReactive.Value = value;
                PlayerPrefs.SetInt("BestScore", value);
            }
        }

        private int _bestScore;
        
        private void Awake()
        {
            AddEventActions(new ()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted},
                { GlobalEventEnum.GameEnded, OnGameFinished},
            });
        }

        private void Start()
        {
            _bestScore = PlayerPrefs.GetInt("BestScore", 0);
            BestScoreReactive.Value = _bestScore;
        }
        
        protected virtual void OnGameStarted()
        {
            ResetScore();
        }
        private void OnGameFinished()
        {
            if (TotalScoreReactive.Value > BestScore) BestScore = TotalScoreReactive.Value;
        }

        public virtual void UpdateScore(int value)
        {
            _currencyManager.Add(CurrencyType.Gold, 1);
            
            TotalScoreReactive.Value += Mathf.RoundToInt(value * _multiplier.Current);
        }

        private void ResetScore()
        {
            TotalScoreReactive.Value = 0;
        }
    }
}