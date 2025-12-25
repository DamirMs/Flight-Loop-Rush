using UnityEngine;

namespace PT.Tools.ObjectArray
{
    public class LevelObjectArray : ObjectArray
    {
        [Space] [Space] [Space]
        [Header("Level")]

        [Tooltip("Amount of object arrays to create")]
        [Min(0)][SerializeField] private int levelsAmount = 1;

        [Tooltip("Amount of new Level Parts added in a sequence for object array")]
        [Min(0)][SerializeField] private int progressionValue = 0;

        [Tooltip("Direction of Levels object arrays")]
        [SerializeField] private Vector3 levelDirection = Vector3.zero;

        [Tooltip("Parent of the Level")]
        [SerializeField] private GameObject levelTemplatePrefab;

        [SerializeField] private GameObject levelStartPrefab;
        [SerializeField] private GameObject flagsPrefab;
        [SerializeField] private GameObject[] gatesPrefabs;

        public int LevelsAmount => levelsAmount;
        public int ProgressionValue => progressionValue;
        public Vector3 LevelDirection => levelDirection;
        public GameObject LevelTemplatePrefab => levelTemplatePrefab;
        public GameObject LevelStartPrefab => levelStartPrefab;
        public GameObject FlagsPrefab => flagsPrefab;
        public GameObject[] GatesPrefabs => gatesPrefabs;
    }
}
