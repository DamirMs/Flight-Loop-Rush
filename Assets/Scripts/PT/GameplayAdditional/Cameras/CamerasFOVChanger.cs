using UnityEngine;

namespace PT.GameplayAdditional.Cameras
{
    public class CamerasFOVChanger : MonoBehaviour
    {
        [SerializeField] private Camera[] cameras;
        [SerializeField] private float orthographicFactor = 3f;
        [SerializeField] private bool changeRuntime;

        private ScreenOrientation _lastOrientation;

        private void Awake()
        {
            _lastOrientation = Screen.orientation;
            UpdateOrto();
        }

        private void Update()
        {
            if (changeRuntime && Screen.orientation != _lastOrientation)
            {
                _lastOrientation = Screen.orientation;
                
                UpdateOrto();
            }
        }

        private void UpdateOrto()
        {
            float screenRatio = (float)Screen.height / Screen.width;
            float orthographicSize = Mathf.Clamp(screenRatio * orthographicFactor, 5.2f, 7.5f);

            foreach (var camera in cameras)
                camera.orthographicSize = orthographicSize;

            DebugManager.Log(DebugCategory.Gameplay, $"Updated camera ortho size: {orthographicSize}");
        }
    }
}