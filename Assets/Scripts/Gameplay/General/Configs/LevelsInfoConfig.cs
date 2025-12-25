using Gameplay.General.Levels;
using UnityEngine;

namespace Gameplay.General.Configs
{
    [CreateAssetMenu(menuName = "Configs/LevelsInfo", fileName = "LevelsInfoConfig")]
    public class LevelsInfoConfig : ScriptableObject
    {
        [SerializeField] private bool savesLevel;
        [Space]
        [SerializeField] private LevelsInfo levelsInfo;
        
        public LevelsInfo LevelsInfo => levelsInfo;
        public bool SavesLevel => savesLevel;
    }
}