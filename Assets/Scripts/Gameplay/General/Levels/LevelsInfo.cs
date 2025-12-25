using System;
using UnityEngine;

namespace Gameplay.General.Levels
{
    [Serializable]
    public class LevelsInfo
    {
        [SerializeField] private Level[] levels;
        
        public Level[] Levels => levels;
    }
}