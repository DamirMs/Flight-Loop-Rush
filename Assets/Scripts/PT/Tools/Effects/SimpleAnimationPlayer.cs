using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace PT.Tools.Effects
{
    public class SimpleAnimationPlayer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private Sprite[] sprites;
        
        protected ISpriteRendererProxy _rendererProxy;
        private Action<GameObject, SimpleAnimationPlayer> _returnAction;
        protected CancellationTokenSource _cts;
        protected bool _isPlaying = false;
        protected bool _isForceFinishing = false;

        protected virtual void Awake()
        {
            _rendererProxy = GetComponent<ISpriteRendererProxy>();
            if (_rendererProxy == null) DebugManager.Log(DebugCategory.Errors, "Missing ISpriteRendererProxy (ImageSpriteProxy or SpriteRendererProxy) on GameObject.");
        }

        protected virtual void OnEnable()
        {
            _isForceFinishing = false;
            if (_rendererProxy != null && sprites.Length > 0)
                _rendererProxy.Sprite = sprites[0];
        }

        protected virtual void OnDisable()
        {
            _cts?.Cancel(); _cts?.Dispose(); _cts = null;
            _isPlaying = false;
        }

        public void Init(Action<GameObject, SimpleAnimationPlayer> returnAction)
        {
            _returnAction = returnAction;
        }
        
        public virtual async UniTask PlayAnimation()
        {
            if (_isPlaying || sprites.Length == 0 || _rendererProxy == null || !gameObject.activeInHierarchy) return;

            _cts = new();
            _isPlaying = true;

            float frameDuration = duration / sprites.Length;

            try
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    if (_isForceFinishing || this == null || !gameObject.activeInHierarchy) break;

                    if (_rendererProxy != null)
                        _rendererProxy.Sprite = sprites[i];

                    await UniTask.Delay(TimeSpan.FromSeconds(frameDuration), cancellationToken: _cts.Token);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                DebugManager.Log(DebugCategory.Errors, $"Animation error: {ex}", LogType.Error);
            }

            if (this != null && gameObject.activeInHierarchy && _rendererProxy != null && sprites.Length > 0)
            {
                _rendererProxy.Sprite = sprites[^1];
            }

            _isPlaying = false;

            if (this != null && gameObject.activeInHierarchy)
                _returnAction?.Invoke(gameObject, this);
        }

        public void ForceFinish()
        {
            _isForceFinishing = true;
            _cts?.Cancel();
        }
    }
}
