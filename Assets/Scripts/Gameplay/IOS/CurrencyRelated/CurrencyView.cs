using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.IOS.CurrencyRelated
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private CurrencyType type;
        [SerializeField] private TextMeshProUGUI text;

        [Inject] private CurrencyManager _currencyManager;
        
        private CompositeDisposable _disposable = new();
        
        private void OnEnable()
        {
            _currencyManager.GetReactive(type)
                .Subscribe(val => text.text = val.ToString())
                .AddTo(_disposable);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}