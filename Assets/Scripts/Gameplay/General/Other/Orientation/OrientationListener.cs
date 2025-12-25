using PT.Tools.EventListener;
using UnityEngine;

namespace Gameplay.General.Other.Orientation
{
    public class OrientationListener : MonoBehaviour
    {
        private ScreenOrientation _lastOrientation;

        private void Start()
        {
            _lastOrientation = Screen.orientation;
        }

        private void Update()
        {
            if (Screen.orientation != _lastOrientation)
            {
                _lastOrientation = Screen.orientation;
                GlobalEventBus.On(GlobalEventEnum.OrientationChanged);
            }
        }
    }
}