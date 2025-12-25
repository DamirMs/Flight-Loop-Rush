using UnityEngine;
using UnityEditor;

namespace PT.Tools.RandomCreator
{
    [CustomEditor(typeof(RandomInitializer))]
    public class RandomCreator : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            RandomInitializer randomInitializer = (RandomInitializer)target;

            if (GUILayout.Button("Clear Objects", GUILayout.Width(100), GUILayout.Height(20)))
            {
                ClearObjects(randomInitializer.transform);
            }

            if (GUILayout.Button("Create Objects"))
            {
                ClearObjects(randomInitializer.transform);

                var size = CalculateMeshSize(randomInitializer.GetMesh());

                CreateObjects(size[0], size[1], randomInitializer.ObjectsToInstantiate, randomInitializer.transform, 
                    randomInitializer.SpawnChance, randomInitializer.Step, randomInitializer.YOffset, randomInitializer.Rotated);
            }

            serializedObject.ApplyModifiedProperties();
        }
        private Vector3[] CalculateMeshSize(MeshFilter meshFilter)
        {
            var bounds = meshFilter.sharedMesh.bounds;

            var min = meshFilter.transform.TransformPoint(bounds.min);
            var max = meshFilter.transform.TransformPoint(bounds.max);

            return new Vector3[2] { min, max };
        }

        private void CreateObjects(Vector3 startPos, Vector3 endPos, GameObject[] objs, Transform parent, float spawnChance, float step, float yOffset, bool rotated)
        {
            for (float i = startPos.x; i < endPos.x; i += 1 + step)
            {
                for (float j = startPos.z; j < endPos.z; j += 1 + step)
                {
                    if (spawnChance > Random.value)
                    {
                        var pos = new Vector3(i + Random.value, yOffset, j + Random.value);
                        var rot = rotated ? Quaternion.Euler(0, Random.Range(0, 360), 0) : Quaternion.identity;

                        var gO = (GameObject)PrefabUtility.InstantiatePrefab(objs[objs.Length == 1 ? 0 : Random.Range(0, objs.Length)], parent);
                        gO.transform.SetPositionAndRotation(pos, rot);
                    }
                }
            }
        }

        private void ClearObjects(Transform parentTransform)
        {
            while (parentTransform.childCount > 0)
                foreach (Transform childTransform in parentTransform)
                    DestroyImmediate(childTransform.gameObject);
        }
    }
}