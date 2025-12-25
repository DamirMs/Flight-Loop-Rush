using UnityEditor;
using UnityEngine;

namespace PT.Tools.ObjectArray
{
    [CustomEditor(typeof(ObjectArray))]
    public class ObjectArrayBuilder : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            ObjectArray objectArray = (ObjectArray)target;

            if (GUILayout.Button("Clear Objects", GUILayout.Width(100), GUILayout.Height(20)))
            {
                ClearObjects(objectArray.transform);
            }

            if (GUILayout.Button("Create array"))
            {
                ClearObjects(objectArray.transform);

                CreateArray(objectArray.Prefabs, objectArray.Direction, 
                    objectArray.Amount, objectArray.Randomize, objectArray.transform);
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected private void CreateArray(GameObject[] prefabs, Vector3 direction, int n, bool randomize, Transform parent)
        {
            int prefabsN = prefabs.Length;

            var objectsInfos = GetObjectArrayInfo(prefabs, prefabsN, direction);
            SpawnArrayObjects(objectsInfos, prefabsN, n, randomize, parent);
        }
        protected private ObjectArrayPrefabInfo[] GetObjectArrayInfo(GameObject[] prefabs, int prefabsN, Vector3 direction)
        {
            var objectsInfos = new ObjectArrayPrefabInfo[prefabsN];

            for (int i = 0; i < prefabsN; i++)
            {
                var renderers = prefabs[i].GetComponentsInChildren<Renderer>();
                var bounds = new Bounds();
                foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);
                var size = bounds.size;

                var scaledDirecton = new Vector3(direction.x * size.x, direction.y * size.y, direction.z * size.z);

                objectsInfos[i] = new ObjectArrayPrefabInfo(prefabs[i], scaledDirecton);
            }

            return objectsInfos;
        }
        protected private void SpawnArrayObjects(ObjectArrayPrefabInfo[] objectsInfos, int prefabsN, int n, bool randomize, Transform parent)
        {
            for (int i = 0; i < n * prefabsN; i++)
            {
                var curObjectInfo = objectsInfos[randomize ? Random.Range(0, prefabsN) : i % prefabsN];

                var pos = curObjectInfo.ScaledDirection * i + curObjectInfo.GetPos();
                var rot = curObjectInfo.GetRotation();

                var gO = (GameObject)PrefabUtility.InstantiatePrefab(curObjectInfo.Prefab, parent);
                gO.transform.SetPositionAndRotation(pos, rot);
            }
        }


        protected private void ClearObjects(Transform parentTransform)
        {
            while (parentTransform.childCount > 0)
                foreach (Transform childTransform in parentTransform)
                    DestroyImmediate(childTransform.gameObject);
        }
    }

    class ObjectArrayPrefabInfo
    {
        public GameObject Prefab;

        public Vector3 ScaledDirection;

        public Vector3 GetPos()
        {
            return Prefab.transform.position;
        }
        public Quaternion GetRotation()
        {
            return Prefab.transform.rotation;
        }

        public ObjectArrayPrefabInfo(GameObject prefab, Vector3 direction)
        {
            Prefab = prefab; ScaledDirection = direction;
        }
    }
}
