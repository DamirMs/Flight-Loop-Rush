using PT.Tools.EventListener;
using UnityEngine;

namespace Gameplay.General.Other.Orientation
{
    public class OrientationToggler : MonoBehaviourEventListener
    {
        [SerializeField] private GameObject portraitLayout;
        [SerializeField] private GameObject landscapeLayout;

        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.OrientationChanged, OnOrientationChanged},
            });
            UpdateOrientation(Screen.orientation);
            UpdateOrientation(Screen.orientation);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            UpdateOrientation(Screen.orientation);
        }

        private void OnOrientationChanged()
        {
            UpdateOrientation(Screen.orientation);
        }

        private void UpdateOrientation(ScreenOrientation orientation)
        {
            var isPortrait = orientation == ScreenOrientation.Portrait || orientation == ScreenOrientation.PortraitUpsideDown;

            if (portraitLayout) portraitLayout.SetActive(isPortrait);
            if (landscapeLayout) landscapeLayout.SetActive(!isPortrait);
        }
    }
}