using UnityEngine;

namespace Gameplay.General.Other.TimeRelated
{
    public class TimeToggler
    {
        public void ToggleTime(bool toggle)
        {
            Time.timeScale = toggle ? 1 : 0;
            
            DebugManager.Log(DebugCategory.Misc, "Time toggled: " + (toggle ? "Enabled" : "Disabled"));
        }
    }
}