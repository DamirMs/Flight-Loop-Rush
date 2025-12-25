using Cysharp.Threading.Tasks;
using PT.Tools.EventListener;
using UnityEngine;

namespace PT.Tools.Windows
{
    public interface IWindow
    {
        WindowTypeEnum Type { get; }
        bool IsOpen { get; }

        UniTask OpenAsync(object data = null);
        UniTask CloseAsync();
    }
    
    public abstract class WindowBase : MonoBehaviourEventListener, IWindow
    {
        [SerializeField] private WindowTypeEnum type;
        
        public WindowTypeEnum Type => type;
        public bool IsOpen { get; private set; }

        public async UniTask OpenAsync(object payload = null)
        {
            gameObject.SetActive(true);
            IsOpen = true;
            
            await OnOpen();
        }

        public async UniTask CloseAsync()
        {
            await OnClose();
            
            IsOpen = false;
            gameObject.SetActive(false);
        }
        
        protected virtual void OnInit() {}
        protected virtual async UniTask OnOpen() => await UniTask.CompletedTask;
        protected virtual async UniTask OnClose() => await UniTask.CompletedTask;
        
    }
}