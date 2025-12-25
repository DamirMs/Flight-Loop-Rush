using System;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.General.Score
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private ScoreTextInfo totalScoreText;
        [SerializeField] private ScoreTextInfo bestScoreText;

        [Serializable]
        class ScoreTextInfo
        {
            [SerializeField] private string additionalText;
            [SerializeField] private TMP_Text scoreText;
            
            public string AdditionalText => additionalText;
            public TMP_Text ScoreText => scoreText;
        }
        
        [Inject] private ScoreManager _scoreManager;
        
        private CompositeDisposable _disposables = new();

        private void Awake()
        {
            if (totalScoreText.ScoreText) 
                _scoreManager.TotalScoreReactive
                    .Subscribe(score => totalScoreText.ScoreText.text = totalScoreText.AdditionalText + score.ToString())
                    .AddTo(_disposables);

            if (bestScoreText.ScoreText) 
                _scoreManager.BestScoreReactive
                    .Subscribe(score => bestScoreText.ScoreText.text = bestScoreText.AdditionalText + score.ToString())
                    .AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}