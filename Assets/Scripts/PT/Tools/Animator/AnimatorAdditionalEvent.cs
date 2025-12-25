using UnityEngine;
using UnityEngine.Events;

namespace PT.Tools.AnimatorTools
{
    public class AnimatorAdditionalEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent additionalAnimationEvent;

        public void OnInvokeAdditionalAnimationEvent() => additionalAnimationEvent.Invoke();
    }
}