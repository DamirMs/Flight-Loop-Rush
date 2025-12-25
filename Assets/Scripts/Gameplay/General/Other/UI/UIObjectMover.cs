using DG.Tweening;
using UnityEngine;

namespace Gameplay.General.Other.UI
{
    public class UIObjectMover : MonoBehaviour
    {
        [SerializeField] private RectTransform target;
        [SerializeField] private float moveDistance = 20f;
        [SerializeField] private float moveDuration = 1f;

        private Tween _moveTween;

        private void OnEnable() 
        {
            _moveTween = target
                .DOAnchorPosY(target.anchoredPosition.y + moveDistance, moveDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void OnDisable()
        {
            _moveTween?.Kill();
            _moveTween = null;
        }
    }
}