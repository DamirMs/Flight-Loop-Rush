using UnityEngine;

namespace PT.Tools.RandomCreator
{
    public class RandomInitializer : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;

        [SerializeField] private GameObject[] objectsToInstantiate;
        [Range(0.01f, 1)][SerializeField] private float spawnChance = 1;
        [Min(0)][SerializeField] private float step = 1;
        [SerializeField] private float yOffset = 1;
        [SerializeField] private bool rotated;

        public GameObject[] ObjectsToInstantiate => objectsToInstantiate;
        public float SpawnChance => spawnChance;
        public float Step => step;
        public float YOffset => yOffset;
        public bool Rotated => rotated;

        public MeshFilter GetMesh()
        {
            return meshFilter != null ? meshFilter : GetComponent<MeshFilter>();
        }

        private void OnDrawGizmosSelected()
        {
            MeshFilter meshFilter = GetMesh();

            if (meshFilter != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(meshFilter.transform.position, meshFilter.transform.TransformPoint(meshFilter.sharedMesh.bounds.size));
            }
        }
    }
}
