using DG.Tweening;
using UnityEngine;

namespace Gameplay.General.Other.UI
{
    public class UIObjectRotator : MonoBehaviour
    {
        [SerializeField] private RectTransform target;
        [SerializeField] private float rotateDuration = 1f;

        private Tween _rotateTween;

        private void OnEnable()
        {
            _rotateTween = target
                .DORotate(new Vector3(0, 0, 360), rotateDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }

        private void OnDisable()
        {
            _rotateTween?.Kill();
            _rotateTween = null;
        }
    }
}