using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Gameplay.Current.ChickenSkies.Math
{
    public abstract class BaseMathWindow : WindowBase
    {
        [Header("UI")]
        [SerializeField] protected Image timerFill;
        [SerializeField] protected TextMeshProUGUI questionText;
        [SerializeField] protected Button[] answerButtons;
        [SerializeField] protected TextMeshProUGUI[] answerTexts;

        [Header("Timing")]
        [SerializeField] protected float startTime = 5f;
        [SerializeField] protected float timeMultiplier = 0.9f;

        protected float _currentTime;
        protected int _correctAnswer;
        protected bool _answered;

        protected CancellationTokenSource _cts;

        protected override void OnEnable()
        {
            base.OnEnable();

            _cts?.Cancel();
            _cts = new();

            _answered = false;
            timerFill.fillAmount = 1f;

            GenerateTask();
            RunTimer(_cts.Token).Forget();
        }

        protected override void OnDisable()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            base.OnDisable();
        }

        protected void GenerateTask()
        {
            int a = Random.Range(-10, 20);
            int b = Random.Range(-10, 20);

            _correctAnswer = a + b;
            questionText.text = $"{a} + {b} = ?";

            int correctIndex = Random.Range(0, answerButtons.Length);

            for (int i = 0; i < answerButtons.Length; i++)
            {
                int value = i == correctIndex
                    ? _correctAnswer
                    : _correctAnswer + Random.Range(-5, 6);

                if (value == _correctAnswer && i != correctIndex)
                    value += 1;

                int cached = value;

                answerTexts[i].text = cached.ToString();
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => Answer(cached));
            }
        }

        protected void Answer(int value)
        {
            if (_answered) return;
            _answered = true;

            _cts?.Cancel();

            if (value == _correctAnswer) OnCorrect();
            else OnWrong();
        }

        protected async UniTaskVoid RunTimer(CancellationToken token)
        {
            float timer = _currentTime <= 0 ? startTime : _currentTime;
            float t = timer;

            try
            {
                while (t > 0f && !_answered)
                {
                    token.ThrowIfCancellationRequested();

                    t -= Time.unscaledDeltaTime;
                    timerFill.fillAmount = Mathf.Clamp01(t / timer);

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }

            if (_answered) return;

            _answered = true;
            OnWrong();
        }

        protected abstract void OnCorrect();
        protected abstract void OnWrong();
    }
}