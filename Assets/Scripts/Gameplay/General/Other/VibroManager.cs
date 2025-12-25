using PT.Tools.EventListener;
using UnityEngine;

namespace Gameplay.General.Other
{
    public class VibroManager : MonoBehaviourEventListener
    {
        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.LevelStarted, Vibrate },
            });
        }

        public void Vibrate()
        {
            if (PlayerPrefs.GetInt("VibrationEnabled", 1) == 1)
            {
                Handheld.Vibrate();
            }
        }
    }
}