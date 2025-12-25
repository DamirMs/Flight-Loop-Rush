using UnityEngine;

namespace PT.Tools.Other
{
    public class SlowmoTestManager : MonoBehaviour
    {
        [SerializeField] private KeyCode toggleKey = KeyCode.F4;
        [SerializeField, Range(0f, 1f)] private float slowmoScale = 0.2f;
        
        private bool _isSlowmo = false;
        private float _normalTimeScale = 1f;

        private void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || Application.isEditor == false) return;

            if (Input.GetKeyDown(toggleKey))
            {
                _isSlowmo = !_isSlowmo;
                Time.timeScale = _isSlowmo ? slowmoScale : _normalTimeScale;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            Time.timeScale = _normalTimeScale;
            Time.fixedDeltaTime = 0.02f;
#endif
        }
    }
}