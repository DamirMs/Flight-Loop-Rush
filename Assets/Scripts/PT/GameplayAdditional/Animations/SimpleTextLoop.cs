using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PT.GameplayAdditional.Animations
{
    public class SimpleTextLoop : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private bool _keepStartText = true;
        [SerializeField] private string[] _loopTexts;
        [SerializeField] private float _loopDuration = 0.5f;

        private string _startText = "";
        private int _currentTextIndex;
        private bool _isLooping;

        private void OnDisable()
        {
            StopLoop();
        }
        private void OnDestroy()
        {
            StopLoop();
        }

        public void StartLoop()
        {
            if (_isLooping) return;

            if (_startText == "") _startText = _text.text;

            _isLooping = true;
            LoopText().Forget();
        }

        public void StopLoop()
        {
            _isLooping = false;
            if (_startText != "") _text.text = _startText;
        }

        private async UniTask LoopText()
        {
            while (_isLooping)
            {
                SetText(_loopTexts[_currentTextIndex % _loopTexts.Length]);

                _currentTextIndex++;
                await UniTask.Delay(TimeSpan.FromSeconds(_loopDuration), cancellationToken: this.GetCancellationTokenOnDestroy());
            }
        }

        private void SetText(string text) => _text.text = (_keepStartText ? _startText : "") + text;
    }
}
