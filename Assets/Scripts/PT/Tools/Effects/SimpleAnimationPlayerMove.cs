using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PT.Tools.Effects
{
    public class SimpleAnimationPlayerMove : SimpleAnimationPlayer
    {
        [Header("Move Settings")]
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private Vector2 moveOffset;
        [SerializeField] private Sprite[] moveSprites;

        private Vector2 _initialPosition;

        protected override void OnEnable()
        {
            base.OnEnable();
            _rendererProxy.Sprite = moveSprites.Length > 0 ? moveSprites[0] : null;
            _initialPosition = transform.position;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            transform.position = _initialPosition;
        }

        public override async UniTask PlayAnimation()
        {
            if (_isPlaying || moveSprites.Length == 0) return;

            _cts = new();
            _isPlaying = true;

            float frameDuration = moveDuration / moveSprites.Length;
            float timer = 0;
            int curFrame = 0;

            var startPosition = _initialPosition + moveOffset;
            var targetPosition = _initialPosition;

            try
            {
                while (timer < moveDuration)
                {
                    if (_isForceFinishing) break;

                    timer += Time.deltaTime;
                    float t = Mathf.Clamp01(timer / moveDuration);
                    transform.position = Vector2.Lerp(startPosition, targetPosition, t);

                    if (curFrame < moveSprites.Length - 1 && timer >= (curFrame + 1) * frameDuration)
                    {
                        curFrame++;
                        _rendererProxy.Sprite = moveSprites[curFrame];
                    }

                    await UniTask.Yield(cancellationToken: _cts.Token);
                }
            }
            catch (OperationCanceledException) { }

            transform.position = targetPosition;
            _rendererProxy.Sprite = moveSprites[^1];

            _isPlaying = false;
            await base.PlayAnimation(); // play base animation
        }
    }

}