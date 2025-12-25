using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PT.Tools.EventListener;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PT.Logic.PersistentScene
{
    public enum SceneNameEnum
    {
        Menu = 1, 
        Game = 2,
    }

    public class SceneLoadManager : MonoBehaviourEventListener
    {
        public event Action<bool> ToggleSceneLoad;
        
        public ReactiveProperty<float> LoadFill { get; } = new();

        private CancellationTokenSource _loadingCancellationTokenSource;

        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.AppModeChosen, OnAppModeChosen },
            });
        }

        public async UniTask LoadScene(SceneNameEnum sceneNameToOpen, SceneNameEnum sceneNameToClose)
        {
            SceneManager.UnloadSceneAsync((int)sceneNameToClose);

            await LoadScene(sceneNameToOpen);
        }
        public async UniTask LoadScene(SceneNameEnum sceneName)
        {
            ToggleLoad(true);

            await LoadSceneAsync((int)sceneName, sceneName == SceneNameEnum.Menu);
        }

        public void SceneInitialized()
        {
            DebugManager.Log(DebugCategory.Points, "Menu loaded", LogType.Assert);

            ToggleLoad(false);
        }
        
        private async UniTask LoadSceneAsync(int sceneIndex, bool waitForSceneInit)
        {
            LoadFill.Value = 0;
            _loadingCancellationTokenSource = new();
            var token = _loadingCancellationTokenSource.Token;
            
            try
            {
                var operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
                operation.allowSceneActivation = true;

                while (!operation.isDone)
                {
                    LoadFill.Value = operation.progress;
                    await UniTask.Yield(token);
                    
                    token.ThrowIfCancellationRequested();
                }

                if (!waitForSceneInit)
                    ToggleLoad(false);
            }
            catch (OperationCanceledException)
            {
                DebugManager.Log(DebugCategory.Errors, "Scene loading cancelled", LogType.Error);
            }
            catch (Exception ex)
            {
                DebugManager.Log(DebugCategory.Errors, $"[Scene] Error loading scene {sceneIndex}: {ex}", LogType.Error);
                
                ToggleLoad(false);
            }
        }
        
        private void CancelPreviousLoading()
        {
            _loadingCancellationTokenSource?.Cancel();
            _loadingCancellationTokenSource?.Dispose();
            _loadingCancellationTokenSource = null;
        }

        private void ToggleLoad(bool toggle) => ToggleSceneLoad?.Invoke(toggle);
        
        private void OnDestroy()
        {
            CancelPreviousLoading();
        }

        private void OnAppModeChosen() => ToggleLoad(false);
    }
}
