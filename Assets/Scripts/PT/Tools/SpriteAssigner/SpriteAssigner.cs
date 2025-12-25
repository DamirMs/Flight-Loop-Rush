using UnityEngine;

namespace PT.Tools.SpriteAssigner
{
    [System.Serializable]
    public class KVSpriteAssign
    {
        [SerializeField] private string objectName;
        [SerializeField] private Sprite[] spritesToAssign;

        public string ObjetName => objectName;
        public Sprite[] SpritesToAssign => spritesToAssign;
    }

    public class SpriteAssigner : MonoBehaviour
    {
        [Header("Editor")]
        public GameObject PrefabToAssign;

        [Tooltip("!Equal length of sprites required!")]
        public KVSpriteAssign[] KVSpriteAssigns;
    }
}
