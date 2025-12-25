using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Current.Configs;
using PT.GameplayAdditional.Animations;
using PT.Tools.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Wrapper.Windows
{
    public class LoadingWindow : WindowBase
    {
        [SerializeField] private Image sliderImage;
        [Space]
        [SerializeField] private SimpleTextLoop[] simpleTextLoops;

        [Inject] private GameInfoConfig _gameConfig;
        [Inject(Id = "Persistent")] private WindowsManager _windowsManager;
        
        private Tween _progressTween;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            foreach (var simpleTextLoop in simpleTextLoops) simpleTextLoop.StartLoop();
        }
        
        public void SetProgress(float value) => SetProgress(value, _gameConfig.LoadingAnimationDuration);
        public void SetProgress(float value, float duration)
        {
            if (sliderImage)
            {
                _progressTween?.Kill();
                _progressTween = sliderImage.DOFillAmount(value, duration).SetEase(Ease.OutQuad);
            }
        }
        public void ResetProgress()
        {
            if (sliderImage) sliderImage.fillAmount = 0f;
        }

        protected override async UniTask OnOpen()
        {
            ResetProgress();
            
            await base.OnClose();
        }
        protected override async UniTask OnClose()
        {
            _progressTween?.Kill();
            _progressTween = null;
            
            await base.OnClose();
        }
    }
}