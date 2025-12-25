using Gameplay.Current.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.General.Other.Orientation
{
    public class OrientationSetter : MonoBehaviour
    {
        public enum OrientationMode
        {
            Portrait,
            Landscape,
            Both
        }

        [Inject] private GameInfoConfig _gameInfoConfig;

        private void OnEnable()
        {
            switch (_gameInfoConfig.OrientationMode)
            {
                case OrientationMode.Portrait:
                    Screen.orientation = ScreenOrientation.Portrait;
                    Screen.autorotateToPortrait = true;
                    Screen.autorotateToPortraitUpsideDown = true;
                    Screen.autorotateToLandscapeLeft = false;
                    Screen.autorotateToLandscapeRight = false;
                    break;

                case OrientationMode.Landscape:
                    Screen.orientation = ScreenOrientation.LandscapeRight;
                    Screen.autorotateToPortrait = false;
                    Screen.autorotateToPortraitUpsideDown = false;
                    Screen.autorotateToLandscapeLeft = true;
                    Screen.autorotateToLandscapeRight = true;
                    Screen.orientation = ScreenOrientation.AutoRotation;
                    break;

                case OrientationMode.Both:
                    Screen.orientation = ScreenOrientation.AutoRotation;
                    Screen.autorotateToPortrait = true;
                    Screen.autorotateToPortraitUpsideDown = true;
                    Screen.autorotateToLandscapeLeft = true;
                    Screen.autorotateToLandscapeRight = true;
                    break;
            }
        }
    }
}